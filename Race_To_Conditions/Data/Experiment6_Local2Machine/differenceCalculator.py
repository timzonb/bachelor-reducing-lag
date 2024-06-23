import matplotlib.pyplot as plt
import pandas as pd
import numpy as np

df_server = pd.read_csv('SphereServer.csv', delimiter=';', decimal=",")
df_client = pd.read_csv('SphereClient.csv', delimiter=';', decimal=".")

def getSameIndex():
    i = 0
    for x in df_server['ActualTime']:
        i = i + 1
        if(x - df_client['ActualTime'][1] > 0):    
            return i


x = (df_server - df_client)['Position'][getSameIndex():].dropna()

print(x)
