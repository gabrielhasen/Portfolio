#include <stdlib.h>
#include <stdio.h>
#include <vector>
#include <string.h>
#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <sstream>
#include <time.h>
#include <math.h>
#include "mat.h"
#include "rand.h"

using namespace std;

int main(int argc, char *argv[])
{
    initRand();
    int dimensions = atoi(argv[1]);
    Matrix imageInfo("EigenVectors");
    string file = "";
    string picture = "";
    bool picType = false;
    imageInfo.readImagePixmap(file, picture, picType);
    //cout << picType << endl;
    Matrix image("Image");
    image = imageInfo; 
    //imageInfo.print();
    
    if(dimensions < 0)
    {
        imageInfo = imageInfo.transpose();
    }

    double totalDif = 0;
    Matrix imageMean("Mean");
    imageMean = imageInfo.meanVec();
    Matrix imageSDV = imageInfo.stddevVec();

    Matrix copy("copy");
    copy = imageInfo;
    Matrix zScore = copy.subRowVector(imageMean).divRowVector(imageSDV);
    //zScore.print();
    //cout << "test 1" << endl;
    //cout << endl; double totalDif = 0;
    Matrix savedImageInfo = imageInfo;

//get eigen values of imageInfo
    imageInfo = zScore.cov();            //makes imageInfo a square matrix
    Matrix eigenValues("EigenValues");
    eigenValues = imageInfo.eigenSystem();
    //imageInfo.normalize();
    //imageInfo.print();
    //cout << endl;

    //eigenValues.print();
    //cout << endl;

    //extract the dimensions of the matrix into a new matrix
    Matrix extractedDimensions("Encoded");
    extractedDimensions = imageInfo.extract(0, 0, abs(dimensions), imageInfo.numCols());
    extractedDimensions = extractedDimensions.transpose();
        //extractedDimensions.print();

    //apply steps backwards 
    Matrix uncompress("Decoded");
    uncompress = zScore.dot(extractedDimensions);

    uncompress = uncompress.dotT(extractedDimensions);

    Matrix newUncompress = uncompress.mulRowVector(imageSDV).addRowVector(imageMean);

    //newUncompress.print();

    if(picType == 0)
    {
        newUncompress.writeImagePgm("z.pgm","");
    }
    else
    {
        newUncompress.writeImagePpm("z.ppm","");
    }
    

    image.printSize();
    imageMean.printSize();
    imageInfo.printSize(); 
    eigenValues.printSize(); 
    extractedDimensions.printSize();
    uncompress.printSize();
    totalDif = uncompress.dist2(&image);
    cout << "Dist: " << totalDif << endl;

}