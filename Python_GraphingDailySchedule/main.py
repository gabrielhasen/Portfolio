from dataStore import Information

def main():
   newInfo = Information(11,3,2019)
   print(newInfo.day,newInfo.month,newInfo.year)
   choice = str()
   while choice != "exit":
      choice = raw_input(">> ")
      if choice == "read":
         read(newInfo)
      elif choice == "write":
         write(newInfo)
      elif choice == "update":
         update(newInfo)
      elif choice == "happiness":
         happiness(newInfo)
      elif choice == "homework":
         homework(newInfo)
      elif choice == "study":
         study(newInfo)
      elif choice == "programming":
         programming(newInfo)
      elif choice == "workout":
         workout(newInfo)
      elif choice == "videogames":
         videogames(newInfo)
      elif choice == "dnd":
         dnd(newInfo)
      elif choice == "work":
         work(newInfo)
      elif choice == "socializing":
         socializing(newInfo)
      elif choice == "reading":
         reading(newInfo)
      elif choice == "graph":
         name = raw_input("Name: ")
         graph(newInfo,name)
      else:
         print("Invalid Argument\n")

def update(info):
   choice = raw_input("Happiness: ")
   info.setHappiness(choice)
   choice = raw_input("Homework: ")
   info.setHomework(choice)
   choice = raw_input("Study: ")
   info.setStudy(choice)
   choice = raw_input("Programming: ")
   info.setProgramming(choice)
   choice = raw_input("Workout: ")
   info.setWorkout(choice)
   choice = raw_input("Video Games: ")
   info.setVideogames(choice)
   choice = raw_input("D&D: ")
   info.setDnd(choice)
   choice = raw_input("Work: ")
   info.setWork(choice)
   choice = raw_input("Socializing: ")
   info.setSocializing(choice)
   choice = raw_input("Reading: ")
   info.setReading(choice)

def happiness(info):
   choice = raw_input("Happiness: ")
   info.setHappiness(choice)

def homework(info):
   choice = raw_input("Homework: ")
   info.setHomework(choice)

def study(info):
   choice = raw_input("Study: ")
   info.setStudy(choice)

def programming(info):
   choice = raw_input("Programming: ")
   info.setProgramming(choice)

def workout(info):
   choice = raw_input("Workout: ")
   info.setWorkout(choice)

def videogames(info):
   choice = raw_input("Video Games: ")
   info.setVideogames(choice)

def dnd(info):
   choice = raw_input("D&D: ")
   info.setDnd(choice)

def work(info):
   choice = raw_input("Work: ")
   info.setWork(choice)

def socializing(info):
   choice = raw_input("Socializing: ")
   info.setSocializing(choice)

def reading(info):
   choice = raw_input("Reading: ")
   info.setReading(choice)


def read(info):
   in_f = open('today.txt','r')
   info.day = int(in_f.readline())
   info.month = int(in_f.readline())
   info.year = int(in_f.readline())
   info.happiness = float(in_f.readline())
   info.homework = float(in_f.readline())
   info.study = float(in_f.readline())
   info.programming = float(in_f.readline())
   info.workout = float(in_f.readline())
   info.videogames = float(in_f.readline())
   info.dnd = float(in_f.readline())
   info.work = float(in_f.readline())
   info.socializing = float(in_f.readline())
   info.reading = float(in_f.readline())

def write(info):
   out_f = open('today.txt','w')
   out_f.write(str(info.day))
   out_f.write("\n")
   out_f.write(str(info.month))
   out_f.write("\n")
   out_f.write(str(info.year))
   out_f.write("\n")
   out_f.write(str(info.happiness))
   out_f.write("\n")
   out_f.write(str(info.homework))
   out_f.write("\n")
   out_f.write(str(info.study))
   out_f.write("\n")
   out_f.write(str(info.programming))
   out_f.write("\n")
   out_f.write(str(info.workout))
   out_f.write("\n")
   out_f.write(str(info.videogames))
   out_f.write("\n")
   out_f.write(str(info.dnd))
   out_f.write("\n")
   out_f.write(str(info.work))
   out_f.write("\n")
   out_f.write(str(info.socializing))
   out_f.write("\n")
   out_f.write(str(info.reading))
   out_f.write("\n")

def graph(info, name):
   import plotly.plotly as py
   import plotly.graph_objs as go

   trace0 = go.Bar(
      x=['Happiness', 'Homework', 'Study',
         'Programming', 'Workout', 'Video Games', 
         'D&D', 'Work', 'Socializing', 'Reading'],
      y=[info.getHappiness(), info.getHomework(), info.getStudy(), 
         info.getProgramming(), info.getWorkout(), info.getVideogames(),
         info.getDnd(), info.getWork(), info.getSocializing(),
         info.getReading()],
      marker=dict(
         color=['rgba(135,199,53,1)', 'rgba(95,125,142,1)',
                  'rgba(95,125,142,1)', 'rgba(95,125,142,1)',
                  'rgba(95,125,142,1)', 'rgba(95,125,142,1)',
                  'rgba(95,125,142,1)', 'rgba(95,125,142,1)',
                  'rgba(95,125,142,1)', 'rgba(95,125,142,1)']),
   )

   data = [trace0]
   layout = go.Layout(
      title='Daily Tasks',
   )

   fig = go.Figure(data=data, layout=layout)
   py.plot(fig, filename=name)

# Start of the program
print("Schedule\n==============\n")
main()
