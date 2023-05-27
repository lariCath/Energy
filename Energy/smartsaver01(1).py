import pandas as pd
import numpy as np
from datetime import datetime
import math
######################################################################################

# Funktion, die aus einer Liste (list1) die N Maximalwerte ausgibt. In list3 stehen die Stundenangaben, die zu den Werten in list1 gehören.
# Wird bei Wärmespeicher und E-Autp gebraucht.

def Nmaxelements(list1,list3,N):
    final_list = []
    list1 = list1.copy()
    list2= list1.copy()
    index_list =[]
 
    for i in range(0, N):
        max1 = 0
 
        for j in range(len(list1)):
            if list1[j] > max1:
                max1 = list1[j]
        index1 =  list2.index(max1)
        index1= list3[index1]
        index_list.append(index1)


 
        list1.remove(max1)
        final_list.append(max1)
 
    return final_list,index_list
###################################################################################
# # Summieren zu stündlichen Mittelwerten: # # 
def mean_per_hour(df,column_name):
# Extrahieren der Stunden und Minuten aus UTC Datumsformat
    #Spalte kopieren
    datetime_str = (df['Datum (MESZ)'].copy())
    hours = []
    minutes = []
    # Jedes Element der Spalte auftrennen und Minuten/Stunden extrahieren
    for elem in datetime_str:
        temp  = str(elem)
        time = temp.split('T')[1].split('+')[0]
        hours.append(time[0:2])
        minutes.append(time[3:5])
    # Umwandeln der Stunden von Format 01 auf 1 etc. und umwandeln to int
    for i in range(len(hours)):
        if hours[i][0]=='0':
            hours[i] =hours[i][1]  
    hours = [eval(i) for i in hours]
    #Hinzufügen der Minuten und Stunden zum Original df
    df=df.assign(hour=hours)
    df=df.assign(minute = minutes)

    #print(df)
    #Auswählen der richtigen Datenspalte
    renew = df[column_name]

    #Stundenduplikate aussortieren und wieder sortieren
    hour_set= list(set(hours))
    hour_set.sort()

    # Mittelwerte pro Stunde berechnen
    hourly_values = []
    for elem in hour_set:
        temp_sum = 0
        for i in range(len(renew)):
            if elem == hours[i]:
                if renew[i+1] =='nan':
                    temp_sum = temp_sum
                else:
                    temp_sum = temp_sum + float(renew[i+1])
        hour_value = temp_sum/60
        hourly_values.append(hour_value)
    # Gibt die Stundenwerte richtig sortiere von 0-24 Uhr zurück
    return hourly_values,hour_set
###############################################################################################

# Wärmespeicher: 

def waerme_func(T_waerme,hourly_values,hour_set):
    
    count = 0
    thresh = 60 # dynamisch anpassbar im Idealfall, aber von uns definiert. Mindestanteil an EE, so dass sich Aufheizen lohnt
    min_temp = 15 # dynamisch anpassbar im Idealfall, aber von uns definiert. Außentemperatur, aber der Heizungüberhaupt notwendig

    #Zählen der Stunden bei denen Anteil Renew über thresh liegt
    for elem in hourly_values: 
        if elem>thresh:
            count = count + 1

    #Vergleich mit Außentemp: Wenn in den nächsten zwei Tagen kälter als min_temp, dann heizen sonst nicht.
    if temp[0]>min_temp and temp[1]>min_temp:
        print('Temperatur hoch - kein Aufheizen ')
        max_values = []
        max_hours = []
    # zu wenige Stunden über thresh (<3) dann nicht heizen weil lohnt sich nicht
    elif count <3: 
        print('Kein Aufheizen da Wetter zu schlecht')
        max_values = []
        max_hours = []
    # Wenn beide Bedinungen nicht greifen, dann gebe Werte und zugehörige Stunden aus. So viele wie T_waerme angibt.
    else: 
        max_values, max_hours = Nmaxelements(hourly_values,hour_set, T_waerme)
    return max_values, max_hours

########################################################################################################
#Batteriespeicher
def battery_func(solar_share,wind_share):
    count = 0
    thresh = 30 # Mindestanteil Sonnenenergie, so dass Batteriespeicher durch hauseigene PV geladen wird
    # Zählen der Sonnenenergie reichen Stunden
    for elem in solar_share: 
        if elem>thresh:
            count = count + 1
    count2 = 0
    thresh2 = 30 # Mindestanteil Windenergie, dass sich andernfalls (ohne Sonne) Laden des Batteriespeichers lohnt
    #Zählen der Windenergiereichen Stunden
    for elem in wind_share: 
        if elem>thresh2:
            count2 = count2 + 1

    wind_hours = []
    wind_values = []
    # Wenn die Sonne scheint brauche ich nicht laden
    if count >2:
        print('die sonne scheint und der batteriespeicher lädt sich selber')
        wind_values = []
        wind_hours = []
    # Laden lohnt sich nur wenn dann viel Wind geht
    elif count2 <3:
        print('wir laden nicht weil keine erneuerbare')
        wind_values = []
        wind_hours = []
    # Wenn sich laden lohnt, lade zu allen Stunden in denen der Wind günstig ist
    else: 
        for elem in wind_share:
            if elem > thresh2:
                wind_values.append(elem)
                index1 =  wind_share.index(elem)
                wind_hours.append(index1)
    #print(wind_values,wind_hours)
    return wind_values,wind_hours

#########################################################################################################
# E-Auto

def car_func(T_car,timeframes,hourly_values):
    rel_values = []
    rel_hours = []
    for el in timeframes:
        for i in range(el[0],el[1]):
            rel_values.append(hourly_values[i])
            rel_hours.append(i)

   # print(rel_values, rel_hours)
    max_values,max_hours = Nmaxelements(rel_values,rel_hours,T_car)
    return max_values,max_hours

########################################################################################
# Haushaltsgeräte
def devices_func(T_geraete, timeframes,hourly_values ):
    score_list= []
    start_time = []
    for el in timeframes:
        for i in range(el[0],el[1]-(T_geraete-1)):
            score = 0
            start_time.append(i)
            for j in range(T_geraete):
                score = score + hourly_values[i+j]
            score_list.append(score)
    #print(score_list,start_time)
    max_score = max(score_list)
    #print(max_score)
    index1 = score_list.index(max_score)
    best_time = start_time[index1]
    #print(best_time)
    return max_score, best_time

#################################################################################
#Einlesen der Daten für heute
df = pd.read_csv("ec26.csv")

#Löschen der 1.Zeile da Tabelleninfos und unwichtig
df= df.drop(0)
#print(df)

column_name = 'Anteil EE an Last'
hourly_values,hour_set = mean_per_hour(df,column_name)
#print('hourly values today:',hourly_values, hour_set)

# Beispieldaten

#Temperatur folgende zwei Tage
temp= [17,13]
# Stundenmittelwerte (Prognose) (dies sind die Daten 0-24 Uhr vom 26.05.)
hourly_values = [53.6, 52.8, 51.51, 50.0, 47.6, 45.1, 42.3, 46.2, 57.3, 70.4, 81.59, 89.6, 94.5, 99.0, 99.19, 95.6, 87.73, 73.19, 58.1, 46.265, 39.7, 41.47, 45.7, 49.0]
#hourly_values = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20.0, 30.3, 40.4, 94.3, 93.2, 50.5, 40.9, 30.8, 54.4, 42.3, 38.1, 39.4, 42.4, 43.8]

# Anteil Solar (26.05) als Beispiel
solar_share = [0, 0, 0, 0, 0, 0, 3.7, 15, 30.9, 44.2, 53.0, 58.1, 59, 59.3, 58.2, 56.5, 52.9, 45.8, 29, 14, 6.6, 0.5, 0, 0]
# Anteil Wind gesamt (26.05.)
wind_share = [42, 42, 40, 41, 40, 38, 33, 24, 15, 14, 13, 13, 13, 13, 12, 15, 19, 21, 26, 29, 28, 31, 34, 37]

T_waerme = 5 # Input! Ladedauer Wärmespeicher. Soll vom Nutzer eingegeben werden.
max_values_waerme, max_hours_waerme = waerme_func(T_waerme,hourly_values,hour_set)
# print('Waerme' ,max_values_waermee)
print('Waerme, Stunden' , max_hours_waerme) # Ausgabe der Stunden zu denen Wärmespeicher laden soll oder Meldung dass er nicht laden soll


wind_values, wind_hours = battery_func(solar_share, wind_share)
#print('Battery:',wind_values)
print('Battery, Stunden:',wind_hours)

T_car= 4 # Input! Ladedauer E-Auto. Soll vom Nutzer eingegeben werden
timeframes= [[12,17],[18,20]] # Zeitdauer in der Auto verfügbar ist. Soll vom Nutzer eingegeben werden
max_values, max_hours = car_func(T_car, timeframes, hourly_values)
#print('E-Auto:', max_values)
print('E-Auto, Stunden:', max_hours)

T_geraete = 2 # Laufdauer Haushaltsgerät. Soll vom Nutzer angegeben werden. 
timeframes2 =  [[12,17],[18,20]]# Zeitdauer in der Gerät laufen kann. Soll vom Nutzer angegeben werden
max_score, best_time = devices_func(T_geraete,timeframes,hourly_values)
#print('Geräte:',max_score )
print('Geräte, Stunden:', best_time )


  
