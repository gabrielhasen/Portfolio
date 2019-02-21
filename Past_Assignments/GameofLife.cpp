#include <iostream>
#include <stdlib.h>
#include <time.h>


using namespace std;

struct cell{
  int current;
  int next;
  int numNeighbors;
};

const int width = 70, height = 28;
const double spawnRate = 0.25;

void printMap(cell[][width]);
void initMap(cell[][width]);
void countNeighbors(cell[][width]);
void update(cell[][width]);
void changeMap(cell[][width]);


int main(){
  srand(time(NULL));

  cell map[height][width];
  initMap(map);
  printMap(map);
  
  for( int i = 0; i < 10000; i++ ){

    system("clear");
    countNeighbors(map);
    update(map);
    changeMap(map);
    printMap(map);

    usleep(200000);
  }

}

void update( cell map[][width]){
  for( int h = 0; h < height; h++ ){
    for( int w = 0; w < width; w++ ){
      if( map[h][w].numNeighbors < 2 && map[h][w].current == 1){
	map[h][w].next = 0;
      }
      else if( map[h][w].numNeighbors < 4 && map[h][w].current == 1){
	//cout << "I'm alive!" << endl;
	map[h][w].next = 1;
      }
    else if(map[h][w].numNeighbors >=4 && map[h][w].current == 1){
	map[h][w].next = 0;
      }
      else if( map[h][w].current == 0 && map[h][w].numNeighbors ==3){
	//cout << "I'm alive!" << endl;
	map[h][w].next = 1;
      }
      else{
	map[h][w].next = 0;
      }
    }
  }

}

void countNeighbors( cell map[][width]){
  
  for(int h = 0; h < height; h++){
    for( int w = 0; w < width; w++){
      map[h][w].numNeighbors = 0;
   
      map[h][w].numNeighbors += map[(height+h-1)%height][(width+w-1)%width].current;
      map[h][w].numNeighbors += map[(height+h)%height][(width+w-1)%width].current;
      map[h][w].numNeighbors += map[(height+h+1)%height][(width+w-1)%width].current;
      
      map[h][w].numNeighbors += map[(height+h-1)%height][(width+w+1)%width].current;
      map[h][w].numNeighbors += map[(height+h)%height][(width+w+1)%width].current;
      map[h][w].numNeighbors += map[(height+h+1)%height][(width+w+1)%width].current;
      
      map[h][w].numNeighbors += map[(height+h-1)%height][w].current;
      map[h][w].numNeighbors += map[(height+h+1)%height][w].current;
      
    }
  }
}

void printMap(cell map[][width]){
  for( int w = 0; w < width+2; w++){
    cout << '_';
  }
  cout << endl;

  for( int h = 0; h < height; h++ ){

    cout << '|';

    for( int w = 0; w < width; w++ ){
      if(map[h][w].current == 1){
	cout << '*';
      }
      else{
	cout << ' ';
      }
    }

    cout << '|'<<endl;

  }


  for( int w = 0; w < width+2; w++){
    cout << '-';
  }
  cout << endl;



}

void changeMap(cell map[][width]){
  for( int h = 0; h < height; h++ ){
    for(int w = 0; w< width; w++){
      map[h][w].current = map[h][w].next;
    }
  }

}

void initMap(cell map[][width]){
  for( int h = 0; h < height; h++ ){
    for( int w = 0; w < width; w++ ){
      if( (rand() % 100+1) < (spawnRate*100) ){
	map[h][w].current = 1;
      }
      else{
	map[h][w].current = 0;
      }
    }
  }
}