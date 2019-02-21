/*	Gabriel Hasenoehrl
*	9/20/2018
*	cs 270
*	Wilder
*	hw 2
*/	
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "hw2.h"

char arr[262144][256];
char arrBub[262144][256];
char arrIn[262144][256];
int size = 262144;

int main()
{
	FILE *fptr;
	fptr = fopen("hw2data.txt", "r");
	char ch;
	int i = 0;
	int j = 0;
	while(j != 262144)
	{
		ch = getc(fptr);
		if(i == 255)
		{
			arr[j][i] = '\0';
			arrBub[j][i] = '\0';
			arrIn[j][i] = '\0';
			j++;
			i=0;
		}
		arr[j][i] = ch;
		arrBub[j][i] = ch;
		arrIn[j][i] = ch;
		i++;
	}
	
	//All of my sort calls
	
	printf("Quick Sort:\n");
	clock_t start_t, end_t, total_t;
	start_t = clock();
	quickSort(arr,0,size-1);
	end_t = clock();
	total_t = (double)(end_t - start_t) / (double)CLOCKS_PER_SEC;
	printf("Total Time is: %f\n\n", (double)total_t);
	
	printf("Bubble Sort:\n");
	start_t = clock();
	bubbleSort(arrBub,size);
	end_t = clock();
	total_t = (double)(end_t - start_t) / (double)CLOCKS_PER_SEC;
	printf("Total Time is: %f\n\n", (double)total_t);
	
	
	printf("Inertion Sort:\n");
	start_t = clock();
	insertionSort(arrIn,size);
	end_t = clock();
	total_t = (double)(end_t - start_t) / (double)CLOCKS_PER_SEC;
	printf("Total Time is: %f\n\n", (double)total_t);
}