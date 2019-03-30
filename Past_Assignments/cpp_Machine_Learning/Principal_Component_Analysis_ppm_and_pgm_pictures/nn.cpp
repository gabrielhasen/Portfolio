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
    int step;
    int stride;
    int hiddenNodes;
    int inputs = 2;
    scanf("%d", &step);
    scanf("%d", &stride);
    scanf("%d", &hiddenNodes);
        //cout << "Inputs: " << inputs << "\nHidden Nodes: " << hiddenNodes << endl;

    Matrix Learning("Learning Set");
    Matrix Test("Test Set");

    Learning.read();
    Matrix oldSave = Learning;
    Matrix Normalize;
    Normalize = Learning.normalizeCols();
    Test = Learning.seriesSampleCol(0, step, stride);
    Learning = Test;

    cout << "Sampled Normalized Input:" << endl;
    Test.print();
    //Learning.print();
    //Test.read();

    //Set up final solution matrix to store the learned solution
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
    //LearningInputBias.normalizeCols();
    LearningInputBias.constantCol(inputs,-1);
        //LearningInputBias.print();
    
    Matrix TestInputBias(Test.numRows(), Input.numCols() + 1, 0.0, "Learning Input Bias");
    TestInputBias.insert(&Test, 0, 0);
    //TestInputBias.normalizeCols();
    TestInputBias.constantCol(inputs, -1);
        //TestInputBias.print();

    // Creates the weights
    Matrix Weight(LearningInputBias.numCols(), hiddenNodes, 0.0, "Weight 1");
    Matrix LastValue2 = Weight;
    Weight.rand(-1.0,2.0);
    Matrix Weight2(hiddenNodes + 1, Target.numCols(), 0.0, "Weight 2");
    Matrix LastValue = Weight2;
    Weight2.rand(-1.0,2.0);

    double learningRate = 0.25;
    int counter = 0;
    //LastValue.print();
    //LastValue2.print();
    double momentum = 0.9;

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
        //Y.map(trans);

        //
        // BACKWARDS PHASE
        //

        // FORMULA
        // dy = (y - t) 
        Matrix Y2(Y);
        Matrix Y3(Y);
        // (y - t)
        Matrix filler = (Y.sub(Target));
        Matrix dy = filler;
        dy.scalarMul(1.0/Test.numRows());

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
        Matrix subMatrix = ((HB.Tdot(dy)).scalarMul(learningRate)).add(LastValue.scalarMul(momentum));
        LastValue = subMatrix;
        Weight2.sub(subMatrix);

        // updating hidden bias
        Matrix dh = dhb.extract(0, 0, dhb.numRows(), dhb.numCols() - 1);
        
        // updating weight 1
        Matrix subMatrix2 = ((LearningInputBias.Tdot(dh)).scalarMul(learningRate)).add(LastValue2.scalarMul(momentum));
        LastValue2 = subMatrix2;
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
    //testFinal.map(trans);
    
    // Adjust solutions to be either 1 or 0 based off of new weights applied
    for(int mapCol = 0; mapCol < Target.numCols(); mapCol++)
    {
        //testFinal.mapCol(mapCol,transSolution);
    }
    
    //cout << "Target" << endl;
    testTarget.unnormalizeCols(Normalize);
    //testTarget.print();

    //cout << "Predicted" << endl;
    testFinal.unnormalizeCols(Normalize);
    //testFinal.print();

    Matrix sideBySide(testFinal.numRows(), testFinal.numCols() + testTarget.numCols(), 0.0, "Test");
    sideBySide.insert(&testFinal, 0, 0);
    sideBySide.insert(&testTarget, 0, 1);
    cout << "Est. and Target" << endl;
    sideBySide.print();

    //every element (x1- x2)^2 sqrt
    double totalDif = 0;
    /*int row = 0;
    int col = 0;
    for(int i = 0; i < testFinal.numRows(); i++)
    {
        double est = sideBySide.get(row,col);
        double exact = sideBySide.get(row,col + 1);
        double value = (pow(est - exact, 2.0));
        cout << value << endl;
        row++;
        totalDif += value;
    }*/
    totalDif = testFinal.dist2(&testTarget);
    cout << "Dist: " << totalDif << endl;
}