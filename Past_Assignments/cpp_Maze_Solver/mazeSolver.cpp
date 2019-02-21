/* stack.cpp
 *
 * CS 121.Bolden.........Compiler version...........Gabriel Hasenoehrl
 * 2/14/2017 .................Computer & CPU.............hase2378@vandals.uidaho.edu
 *
 * This program reads a maze from a text file.  It then assigns that maze
 * to a 2d array.  The program reads the 2d array and finds how to navigate
 * through the maze starting from the S and getting to the G, by moving along
 * the maze in free locations.  If it comes to a junction it will then add that
 * location to the stack in order to back track and try each route.  If it reaches
 * a dead end it will then pop off the stack to recall the position it was at
 * at to check the other paths.
 *---------------------------------------------------------------------
 */

#include <iostream>
#include <fstream>

using namespace std;

struct node{		//node for the stack
	int pointx;
	int pointy;
	node *next;
};

class Stack{	
private:
	node* top;
	int count;
public:
	Stack(){		//Initializer for Stack
		top = NULL;
		count = 0;
	}
	void getTop(int &y, int&x){
		node *temp;
		temp = top;
		y = temp -> pointy;
		x = temp -> pointx;
	}
	void pop(){		//Deletes top node
		node *temp;
		temp = top;
		top = top->next;
		delete temp;
	}
	void push(int y, int x){		//Creates top node
		node *temp = new node;
		temp -> pointy = y;
		temp -> pointx = x;
		temp -> next = NULL;
		if(top == NULL){
			top = temp;
			count++;
		}else{
			temp -> next = top;
			top = temp;
			count++;
		}
	}
	void display(){					//Displays the whole stack
			node *temp = top;
			int i = 1;
			while (temp != NULL){
				cout << i << ")  X: " << temp -> pointx << "   Y: " << temp -> pointy << endl;
				temp = temp -> next;
				i++;
			}
	}
};


int main(){
	Stack stack;
	ifstream ifile;					//Reading text file
	ifile.open("maze_3.txt");		//maze_1 or maze_2 or maze_3 (saved txt files)
	int height;						//Creating variables
	int width;
	ifile >> height;
	ifile >> width;
	int xGoal;
	int yGoal;
	char field[width][height];
	int xStart = 0;
	int yStart = 0;
	for(int i = 0; i < width; i++){		//Assigns all values in txt file to 2d array
		for(int j = 0; j < height; j++){
			ifile >> field[i][j];
			if(field[i][j] == 'S'){			//Assigns the stack a starting point
				yStart = i;
				xStart = j;
			}if(field[i][j] == 'G'){		//Sets the goal for the maze
				yGoal = i;
				xGoal = j;
			}
		}
	}
	for(int i = 0; i < width; i++){		//Displays 2d array to see the route the program took.
		for(int j = 0; j < height; j++){
			cout << field[j][i] << " ";
		}cout << endl;
	}
	stack.push(yStart,xStart);
	field[yStart][xStart] = 'x';
	while(xStart != xGoal || yStart != yGoal){	//While not at goal
		//directions keep track if there is a junction
		int directions = 0;
		//these if else statements check if G is next to current position so the program won't take a wrong path
		//when it is right next to the goal
		if(field[yStart-1][xStart] == 'G'){			
			field[xStart][yStart] = 'x';
			yStart--;
			stack.push(xStart,yStart);
			break;
		}else if (field[yStart][xStart+1] == 'G'){
			field[xStart][yStart] = 'x';
			xStart++;
			stack.push(xStart,yStart);
			break;
		}else if(field[yStart+1][xStart] == 'G'){
			field[xStart][yStart] = 'x';
			yStart++;
			stack.push(xStart,yStart);
			break;
		}else if(field[yStart][xStart-1] == 'G'){
			field[xStart][yStart] = 'x';
			xStart--;
			stack.push(xStart,yStart);
			break;
		}
		//adds the different directions around the cell checking if there is a junction
		if(field[yStart+1][xStart] == '.'){
			directions++;
		}
		if(field[yStart-1][xStart] == '.'){
			directions++;
		}
		if(field[yStart][xStart+1] == '.'){
			directions++;
		}
		if(field[yStart][xStart-1] == '.'){
			directions++;
		}
		//if there is a junction it will then push that location to the stack and check down each route
		if(directions > 1){
			stack.push(yStart,xStart);
			field[yStart][xStart] = 'x';
			if(field[yStart-1][xStart] == '.'){
				yStart--;
				field[yStart][xStart] = 'x';
			}
			else if(field[yStart+1][xStart] == '.'){
				yStart++;
				field[yStart][xStart] = 'x';
			}
			else if(field[yStart][xStart+1] == '.'){
				xStart++;
				field[yStart][xStart] = 'x';
			}
			else if(field[yStart][xStart-1] == '.'){
				xStart--;
				field[yStart][xStart] = 'x';
			}
		}
		//if there is no junction it will continue to the next valid location
		else{
			if(field[yStart+1][xStart] == '.'){
				yStart++;
				field[yStart][xStart] = 'x';
			}
			else if(field[yStart-1][xStart] == '.'){
				yStart--;
				field[yStart][xStart] = 'x';
			}
			else if(field[yStart][xStart+1] == '.'){
				xStart++;
				field[yStart][xStart] = 'x';
			}
			else if(field[yStart][xStart-1] == '.'){
				xStart--;
				field[yStart][xStart] = 'x';
			}
			//if there is no valid location to move to, it will then pop off the stack and set the location to where it was pushed
			//onto the stack to try the next route
			if(field[yStart+1][xStart] != '.' && field[yStart-1][xStart] != '.' && field[yStart][xStart+1] != '.' && field[yStart][xStart-1] != '.'){
				if(field[yStart-1][xStart] == 'G'){			
					field[xStart][yStart] = 'x';
					yStart--;
					stack.push(xStart,yStart);
					break;
				}else if (field[yStart][xStart+1] == 'G'){
					field[xStart][yStart] = 'x';
					xStart++;
					stack.push(xStart,yStart);
					break;
				}else if(field[yStart+1][xStart] == 'G'){
					field[xStart][yStart] = 'x';
					yStart++;
					stack.push(xStart,yStart);
					break;
				}else if(field[yStart][xStart-1] == 'G'){
					field[xStart][yStart] = 'x';
					xStart--;
					stack.push(xStart,yStart);
					break;
				}
				field[yStart][xStart] = 'D';	//sets character to 'D' when reaching dead end.
				stack.getTop(yStart,xStart);
				stack.pop();
			}
		}
		//prints out the maze and sets the current location to 'X'( I know it prints it out sideways I accidently flipped my rows and columns)
		field[yStart][xStart] = 'X';
		for(int i = 0; i < width; i++){	
			for(int j = 0; j < height; j++){
				cout << field[i][j] << " ";
			}cout << endl;
		}
		field[yStart][xStart] = 'x';
		cout << endl;
	}
	//prints out if the maze completed
	cout << "Maze Completed" << endl;
	field[yStart][xStart] = 'G';
	for(int i = 0; i < width; i++){	
		for(int j = 0; j < height; j++){
			cout << field[i][j] << " ";
		}cout << endl;
	}
	stack.display();
}