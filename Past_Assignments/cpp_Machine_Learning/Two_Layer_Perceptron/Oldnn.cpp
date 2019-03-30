#include <stdlib.h>
#include <stdio.h>
#include <vector>
#include <string>
#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <sstream>
#include <time.h>
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

int main(int argc, char *argv[])
{

    int numberOfIterations = 3000;

    double (*trans)(double);
    trans = &transfer;

    int inputs;
    scanf("%d", &inputs);
    Matrix Data("Data Set");
    Matrix Test("Test");

    Data.read();
    Test.read();
    Matrix Input = Data.extract(0,0,Data.numRows(),inputs);
    Matrix Target = Data.extract(0,inputs,Data.numRows(),Data.numCols()-inputs);

    Matrix Final(Test.numRows(), Data.numCols(), 0.0, "Final Solution");
    Matrix add = Data.extract(0,0,Test.numRows(),inputs);
    Final.insert(&Test,0,0);

    //Target.print();
    Matrix InputBias(Input.numRows(), Input.numCols() + 1, 0.0, "InputBias");
    Matrix InputBias2(Test.numRows(), Input.numCols() + 1, 0.0, "InputBias2");
    InputBias2.insert(&Test,0,0);
    InputBias.insert(&Input,0,0);

    InputBias.normalizeCols();
    InputBias2.normalizeCols();

    InputBias2.constantCol(inputs, -1);
    InputBias.constantCol(inputs, -1);

    Matrix TestNormalized = InputBias;

    //InputBias.print();
    //InputBias2.print();
    //TestNormalized.print();
    //Data.print();
    //Test.print();
    Matrix Weight(InputBias.numCols(), Data.numCols() - inputs, 0.4, "Weight");
    //Weight.print();
    //cout << endl;

    double learningRate = 0.125;
    int counter = 1;
    Matrix Result("Result");
    while(counter <= numberOfIterations)
    {
        //cout << "\nIteration: " << counter << endl;
        Matrix test = InputBias.dot(Weight);
        Result = test;
        //y
        //test.print();
        //t - y
        //Target.print();
        test.map(trans);
        test.sub(Target);
        // makes it y - t
        test.scalarMul(-1);
        test.print();
        //x (t - y)
        test = InputBias.Tdot(test);
        //test.print();
        //learningrate * (x (y-t))
        test.scalarMul(learningRate);
        //test.print();
        //Weight.print();
        Weight.add(test);
        //Weight.print();
        counter++;
    }
    //cout << "\n\n\n";
    //cout << "Result" << endl;
    //Result.normalizeCols();
    Matrix testFinal = InputBias2.dot(Weight);
    Final.insert(&testFinal,0,inputs);
    for(int mapApply = inputs; mapApply < Data.numCols(); mapApply++)
    {
        Final.mapCol(mapApply,trans);
    }
    Final.print();
    //Result.print();
    //cout << "Target" << endl;
    //Data.print();
    

    //add another column to Test that has bias of -1
    //normalize this Test as well when adding final weights
    //but add bias after normalizing
    
}
