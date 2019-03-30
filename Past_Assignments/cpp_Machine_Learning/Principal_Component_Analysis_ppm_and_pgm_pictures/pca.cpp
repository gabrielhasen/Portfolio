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
        image = image.transpose();
    }

    double totalDif = 0;
    Matrix imageMean("Mean");
    imageMean = imageInfo.meanVec();
    Matrix imageSDV = imageInfo.stddevVec();

    Matrix copy("copy");
    copy = imageInfo;
    Matrix zScore = copy.subRowVector(imageMean);
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
    //imageInfo = imageInfo.transpose();
    extractedDimensions = imageInfo.extract(0, 0, abs(dimensions), image.numCols());
        //extractedDimensions.print();

    //apply steps backwards 
    Matrix uncompress("Decoded");
    uncompress = zScore.dotT(extractedDimensions);
    Matrix Encoded("Encoded");
    Encoded = uncompress;

    uncompress = uncompress.dot(extractedDimensions);

    extractedDimensions = extractedDimensions.transpose();

    Matrix newUncompress = uncompress.addRowVector(imageMean);

    //newUncompress.print();
    if(dimensions < 0)
    {
        newUncompress.transposeSelf();
    }

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
    Encoded.printSize();
    uncompress.printSize();
    if(dimensions < 0)
    {
        newUncompress.transposeSelf();
    }
    totalDif = newUncompress.dist2(image);
    totalDif = double(totalDif / (double(image.numCols())*double(image.numRows())));
    cout << "Per Pixel Dist^2: " << totalDif << endl;

}