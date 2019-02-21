#include <iostream>

using namespace std;

struct node{
	int data;
	node *next;
};

class Que{
	private:
		node *front;
	public:
		Que(){
			front = NULL;
		}
		void pushFront(int d){
			node *temp = new node;
			node *counter;
			temp -> data = d;
			temp -> next = NULL;
			if(front == NULL){
				front = temp;
			}else{
				counter = front;
				while(counter->next != NULL){
					counter = counter->next;
				}
				counter -> next = temp;
				
			}
		}
		
		int popQue(){
			node *temp;
			temp = front;
			front = front->next;
			return temp->data;
			delete temp;
		}
		
		void display(){
			node *temp;
			temp = front;
			while(temp != NULL){
				cout << temp->data << endl;
				temp = temp->next;
			}
		}
};

int main(){
	Que que;
	que.pushFront(1);
	que.pushFront(2);
	que.pushFront(3);
	que.display();
	cout << endl;
	que.popQue();
	que.display();
	
}