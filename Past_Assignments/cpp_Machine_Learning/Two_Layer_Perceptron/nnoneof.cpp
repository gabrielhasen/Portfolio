#include <stdlib.h>
#include <stdio.h>
#include <vector>
#include <string>
#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <sstream>
#include <time.h>
#include <math.h>
#include "mat.h"
#include "rand.h"

using namespace std;

double transfer(double x)
{
    if(x >= .5)
    {
        return 1.0;
    }
    else
    {
        return 0.0;
    }
    
}

double expTransfer(double x)
{
    return (1.0/(1.0+exp(-4 * x)));
}

int main()
{
    initRand();
    int numberOfIterations = 20000;

    double (*trans)(double);
    trans = &expTransfer;

    double (*transSolution)(double);
    transSolution = &transfer;

    // File read in information
    int inputs;
    int hiddenNodes;
    int ofClasses;
    scanf("%d", &inputs);
    scanf("%d", &hiddenNodes);
    scanf("%d", &ofClasses);
        //cout << "Inputs: " << inputs << "\nHidden Nodes: " << hiddenNodes << endl;

    Matrix Learning("Learning Set");
    Matrix Test("Test Set");
    Learning.read();
    Test.read();

    // Set up final solution matrix to store the learned solution
    Matrix Final(Test.numRows(), Learning.numCols(), 0.0, "Final Solution");
    Final.insert(&Test, 0, 0);

    // Extracts learning set of input values
    Matrix Input = Learning.extract(0, 0, Learning.numRows(), inputs);

    // Extract target values
    Matrix Target = Learning.extract(0, inputs, Learning.numRows(), Learning.numCols() - inputs);
    Matrix testTarget = Test.extract(0, inputs, Test.numRows(), Test.numCols() - inputs);

    // Creates input bias for 'learning set' and 'test set'
    Matrix LearningInputBias(Learning.numRows(), Input.numCols() + 1, 0.0, "Learning Input Bias");
    LearningInputBias.insert(&Input, 0, 0);
    LearningInputBias.normalizeCols();
    LearningInputBias.constantCol(inputs,-1);
        //LearningInputBias.print();
    
    Matrix TestInputBias(Test.numRows(), Input.numCols() + 1, 0.0, "Learning Input Bias");
    TestInputBias.insert(&Test, 0, 0);
    TestInputBias.normalizeCols();
    TestInputBias.constantCol(inputs, -1);
        //TestInputBias.print();

    // Creates the weights
    Matrix Weight(LearningInputBias.numCols(), hiddenNodes, 0.0, "Weight 1");
    Weight.rand(-1.0,2.0);
    Matrix Weight2(hiddenNodes + 1, Target.numCols(), 0.0, "Weight 2");
    Weight2.rand(-1.0,2.0);

    double learningRate = 0.153;
    int counter = 0;

    while(counter < numberOfIterations)
    {
        // KEY
        // h    =   results first layer
        // hb   =   hidden bias
        // Y    =   result after first and hidden layer
        // dy   =   Delta Y
        // dhb  =   delta hidden bias

        // Get result for first layer
        Matrix H = LearningInputBias.dot(Weight);

        // Restrict weights from getting out of control
        H.map(trans);

        // Creating matrix with a bias to feed into hidden layer
        Matrix HB(H.numRows(), H.numCols() + 1, 0.0, "HB");
        HB.insert(&H,0,0);
        HB.constantCol(H.numCols(), -1);
        
        Matrix Y = HB.dot(Weight2);
        Y.map(trans);

        //
        // BACKWARDS PHASE
        //

        // FORMULA
        // dy = (y - t) y (1 - y)
        Matrix Y2(Y);
        Matrix Y3(Y);
        // (y - t)
        Matrix filler = (Y.sub(Target));
        // (1 - y)
        Matrix filler2 = (Y3.scalarPreSub(1));
        // (y - t) y (1 - y)
        Matrix dy = ( filler2.mul(filler.mul(Y2)) );

        // FORMULA
        // dhb = hb (1 - bh) (dy . Transpose[w])
        Matrix HB2(HB);
        Matrix HB3(HB);
        // (1 - bh)
        Matrix stuff = (HB2.scalarPreSub(1));
        // (dy. Transpose[w])
        Matrix stuff2 = (dy.dotT(Weight2));
        // hb (1 - bh) (dy . Transpose[w])
        Matrix dhb = ( stuff2.mul(stuff.mul(HB3)) );

        //
        // UPDATE
        //
        
        // updating weight 2
        Matrix subMatrix = (HB.Tdot(dy)).scalarMul(learningRate);
        Weight2.sub(subMatrix);

        // updating hidden bias
        Matrix dh = dhb.extract(0, 0, dhb.numRows(), dhb.numCols() - 1);
        
        // updating weight 1
        Matrix subMatrix2 = (LearningInputBias.Tdot(dh)).scalarMul(learningRate);
        Weight.sub(subMatrix2);

        counter++;
    }

    // Apply these new weight to the testing inputs to get results
    Matrix h = TestInputBias.dot(Weight);
    h.map(trans);

    // Creating hidden bias
    Matrix hb(h.numRows(), h.numCols() + 1, 0.0, "HB");
    hb.insert(&h,0,0);
    hb.constantCol(h.numCols(), -1);

    // Add final 
    Matrix testFinal = hb.dot(Weight2);
    testFinal.map(trans);
    
    // Adjust solutions to be either 1 or 0 based off of new weights applied
    for(int mapCol = 0; mapCol < Target.numCols(); mapCol++)
    {
        testFinal.mapCol(mapCol,transSolution);
    }

    // Uncomment this to check solution ONLY if the 'learning set' and 'test
    // set' are the same
        cout << "Target" << endl;
        testTarget.print();

    cout << "Predicted" << endl;
    //testFinal.print();
    Matrix HighestValue(testFinal.numRows(),1, 0.0, "Highest Value");

    for(int i = 0; i < testFinal.numRows(); i++)
    {
        for(int j = 0; j < ofClasses + 1; j++)
        {
            double row = testFinal.get(i,j);
            if(row == 1.0)
            {
                HighestValue.set(i,0,j);
            }
        }
    }
    HighestValue.print();

    //
    // CONFUSION MATRIX
    //

    for(int i = 0; i < testTarget.numCols(); i++)
    {
        cout << "Confusion Matrix" << endl;
        Matrix Confusion(ofClasses,ofClasses,0.0,"Confusion");
        for(int j = 0; j < testTarget.numRows(); j++)
        {
            double targetCompare = testTarget.get(j,i);
            double finalCompare = HighestValue.get(j,i);
            if(targetCompare == finalCompare)
            {
                if(targetCompare == 0)
                {
                    Confusion.inc(0,0);
                }
                else if(targetCompare == 1)
                {
                    Confusion.inc(1,1);
                }
                else if(targetCompare == 2)
                {
                    Confusion.inc(2,2);
                }
            }
            else if(targetCompare != finalCompare)
            {
                if(targetCompare == 0 && finalCompare == 1)
                {
                    Confusion.inc(1,0);
                }
                else if(targetCompare == 1 && finalCompare == 0)
                {
                    Confusion.inc(0,1);
                }
                else if(targetCompare == 0 && finalCompare == 2)
                {
                    Confusion.inc(2,0);
                }
                else if(targetCompare == 1 && finalCompare == 2)
                {
                    Confusion.inc(2,1);
                }
                else if(targetCompare == 2 && finalCompare == 0)
                {
                    Confusion.inc(0,2);
                }
                else if(targetCompare == 2 && finalCompare == 1)
                {
                    Confusion.inc(1,2);
                }
            }
        }
        Confusion.print();
    }
}