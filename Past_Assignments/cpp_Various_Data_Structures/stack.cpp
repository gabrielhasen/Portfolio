/* stack.cpp
 *
 * CS 121.Bolden.........Compiler version...........Gabriel Hasenoehrl
 * 2/14/2017 .................Computer & CPU.............hase2378@vandals.uidaho.edu
 *
 * This program reads a maze from a text file.  It then assigns that maze
 * to a 2d array.  The program reads the 2d array and finds how to navigate
 * through the maze starting from the S and getting to the G, by moving along
 * the maze in free locations and adding those moves onto a stack.  If the
 * program reaches a dead end it will then backtrack to find the next free move.
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
	void pop(){		//Deletes top node
		node *temp;
		temp = top;
		top = top->next;
		delete temp;
	}
	void push(int x, int y){		//Creates top node
		node *temp = new node;
		temp -> pointx = x;
		temp -> pointy = y;
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
	ifile.open("maze2.txt");
	int height;						//Creating variables
	int width;
	ifile >> height;
	ifile >> width;
	int xGoal;
	int yGoal;
	char field[height][width];
	int xStart = 0;
	int yStart = 0;
	for(int i = 0; i < height; i++){		//Assigns all values in txt file to 2d array
		for(int j = 0; j < width; j++){
			ifile >> field[i][j];
			if(field[i][j] == 'S'){			//Assigns the stack a starting point
				xStart = i;
				yStart = j;
			}if(field[i][j] == 'G'){		//Sets the goal for the maze
				xGoal = i;
				yGoal = j;
			}
		}
	}

	int XsavedPosition;
	int YsavedPosition;
	int routes = 0;
	
	while(xStart != xGoal || yStart != yGoal){		//Starts solving the maze
		//These are checks so if it gets to a space next to 'G' it won't take another route first
		//Instead it will prioritize 'G'
		if(field[xStart-1][yStart] == 'G'){			
			field[xStart][yStart] = 'x';
			xStart--;
			stack.push(xStart,yStart);
			break;
		}else if (field[xStart][yStart+1] == 'G'){
			field[xStart][yStart] = 'x';
			yStart++;
			stack.push(xStart,yStart);
			break;
		}else if(field[xStart+1][yStart] == 'G'){
			field[xStart][yStart] = 'x';
			xStart++;
			stack.push(xStart,yStart);
			break;
		}else if(field[xStart][yStart-1] == 'G'){
			field[xStart][yStart] = 'x';
			yStart--;
			stack.push(xStart,yStart);
			break;
		}
		//Moves in a direction around the maze placing 'x' where it has already been
		if(field[xStart+1][yStart] == 'G' || field[xStart+1][yStart] == '.' || field[xStart+1][yStart] == 'S'){	
			stack.push(xStart,yStart);
			field[xStart][yStart] = 'x';
			xStart++;
		}
		else if(field[xStart-1][yStart] == 'G' || field[xStart-1][yStart] == '.' || field[xStart-1][yStart] == 'S'){
			stack.push(xStart,yStart);
			field[xStart][yStart] = 'x';
			xStart--;
		}
		else if(field[xStart][yStart+1] == 'G'|| field[xStart][yStart+1] == '.' || field[xStart][yStart+1] == 'S'){
			stack.push(xStart,yStart);
			field[xStart][yStart] = 'x';
			yStart++;
		}
		else if(field[xStart][yStart-1] == 'G' || field[xStart][yStart-1] =='.' || field[xStart][yStart-1] =='S'){
			stack.push(xStart,yStart);
			field[xStart][yStart] = 'x';
			yStart--;
		}else{		//If the program gets to a dead end it will then trace back up the x's its previously places until there is a valid '.' to move to.
			field[xStart][yStart] = '#';	//Places a wall when it reaches a dead end
			if(field[xStart-1][yStart] == 'x'){
				xStart--;
			}else if (field[xStart+1][yStart] == 'x'){
				xStart++;
			}else if(field[xStart][yStart+1] == 'x'){
				yStart++;
			}else if(field[xStart][yStart-1] == 'x'){
				yStart--;
			}
			stack.pop();
		}
		for(int i = 0; i < height; i++){		//Displays 2d array to see the route the program took.
			for(int j = 0; j < width; j++){
				cout << field[i][j] << " ";
			}cout << endl;
		}
		cout << xStart << " " << yStart << endl;	//Displays coordinates of current position
		cout << endl;
	}
	//Displays 2d array 1 more times since loop breaks when it gets to the goal
	for(int i = 0; i < height; i++){		
		for(int j = 0; j < width; j++){
			cout << field[i][j] << " ";
		}cout << endl;
	}
	cout << xStart << " " << yStart << endl;	//Displays coordinates of current position
	cout << endl;
	cout << "Maze Completed" << endl;
	stack.display();							//Displays the stack after the maze is completed, showing the moves the program made to solve the maze.
}