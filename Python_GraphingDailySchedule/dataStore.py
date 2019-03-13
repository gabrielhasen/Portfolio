class Information:
    def __init__(self,day,month,year):
        self.day = day
        self.month = month
        self.year = year
        self.happiness = 0.0
        self.homework = 0.0
        self.study = 0.0
        self.programming = 0.0
        self.workout = 0.0
        self.videogames = 0.0
        self.dnd = 0.0
        self.work = 0.0
        self.socializing = 0.0
        self.reading = 0.0

    def setHappiness(self, happiness):
        self.happiness = happiness

    def setHomework(self,homework):
        self.homework = homework
    
    def setStudy(self, study):
        self.study = study

    def setProgramming(self, programming):
        self.programming = programming
    
    def setWorkout(self, workout):
        self.workout = workout

    def setVideogames(self, videogames):
        self.videogames = videogames

    def setDnd(self, dnd):
        self.dnd = dnd

    def setWork(self, work):
        self.work = work

    def setSocializing(self, socializing):
        self.socializing = socializing

    def setReading(self, reading):
        self.reading = reading

    def getHappiness(self):
        return self.happiness

    def getHomework(self):
        return self.homework

    def getStudy(self):
        return self.study

    def getProgramming(self):
        return self.programming

    def getWorkout(self):
        return self.workout

    def getVideogames(self):
        return self.videogames

    def getDnd(self):
        return self.dnd

    def getWork(self):
        return self.work

    def getSocializing(self):
        return self.socializing

    def getReading(self):
        return self.reading