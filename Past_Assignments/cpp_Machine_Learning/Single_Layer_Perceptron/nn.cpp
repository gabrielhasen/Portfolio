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
    int numberOfIterations = 200;

    double (*trans)(double);
    trans = &expTransfer;

    double (*transSolution)(double);
    transSolution = &transfer;

    // File read in information
    int inputs;
    int hiddenNodes;
    scanf("%d", &inputs);

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
    Matrix Weight(LearningInputBias.numCols(), Learning.numCols() - inputs, 0.4, "Weight 1");

    double learningRate = 0.123;
    int counter = 0;

    // FORMULA FOR THIS LOOP
    // newWeight = (learningRate * x(y-t))
    while(counter < numberOfIterations)
    {
        // Get result for 'y'
        Matrix H = LearningInputBias.dot(Weight);

        // Restrict weights from getting out of control
        H.map(trans);

        // (t - y)  
        // This is to preserve the matrix as the sub function overwrites
        // and you want to maintain the integrity of the target matrix
        H.sub(Target);


        // Change to (y - t)
        // Change on how we want this subtraction to take place
        H.scalarMul(-1);

        // x(y - t)
        H = LearningInputBias.Tdot(H);

        // LearningRate * x(y - t)
        H.scalarMul(learningRate);

        // Add to 'Weight' matrix to get new weights as this formula so far
        // is just a change by a small amount and must be ran multiple times
        Weight.add(H);

        //Error checking to see how weights are changing
            //Weight.print();


        counter++;
    }

    // Apply this new weight to the testing inputs to get results
    Matrix testFinal = TestInputBias.dot(Weight);
    testFinal.map(trans);
    Final.insert(&testFinal, 0, inputs);
    
    // Adjust solutions to be either 1 or 0 based off of new weights applied
    for(int mapCol = inputs; mapCol < Learning.numCols(); mapCol++)
    {
        Final.mapCol(mapCol,transSolution);
    }

    Final.print();

    // Uncomment this to check solution ONLY if the 'learning set' and 'test
    // set' are the same
        //Learning.print();
}