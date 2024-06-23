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

def getEndIndex():
    i = 0
    for x in df_server['ActualTime']:
        i = i + 1
        if(x - df_client['ActualTime'][len(df_client['ActualTime']) - 1] > 0):    
            return i



sameIndex = getSameIndex()

x = []
y = []

x_Server = []
y_Server = []

x_Client = []
y_Client = []

x_timeDifference = []
y_timeDifference = []

i = sameIndex
serAdd = 0

print(len(df_client['ActualTime']))
print(len(df_server['ActualTime']))

exit = False

nanoToSeconds = float(1000000000)

while i < len(df_client['ActualTime']):
    # while df_server['ActualTime'][i + serAdd] - df_client['ActualTime'][i - sameIndex] > 0:
    #     print(i)
    #     print(sameIndex)
    #     print(serAdd)
    #     print(i + serAdd)
    #     print(df_server['ActualTime'][i + serAdd] - df_client['ActualTime'][i - sameIndex])
    #     print('------------------')
    #     serAdd = serAdd - 1
        
    #     print(df_server['ActualTime'][i + serAdd] - df_client['ActualTime'][i - sameIndex])
    #     print('------------------')
    #     if(i + serAdd >= len(df_server['ActualTime'])):
    #         exit = True
    #         break
    # if(exit):
    #     break

    x.append(df_server['ActualTime'][i + serAdd])
    y.append(df_server['Position'][i + serAdd] - df_client['Position'][i - sameIndex])
    
    x_timeDifference.append(df_server['ActualTime'][i + serAdd])
    y_timeDifference.append(df_server['ActualTime'][i + serAdd] - df_client['ActualTime'][i - sameIndex])
 
    x_Server.append(df_server['ActualTime'][i + serAdd])
    y_Server.append(df_server['Position'][i + serAdd])
 
    x_Client.append(df_client['ActualTime'][i - sameIndex])
    y_Client.append(df_client['Position'][i - sameIndex])

    i = i + 1


#figure, axis = plt.subplots(1, 2)


plt.scatter(x, y, alpha=0.3, color='m')

plt.axhline(y=0.0, color='k', linestyle='-')

# plt.scatter(x_timeDifference, y_timeDifference, alpha=0.1, color='g', label='time difference')
plt.scatter(x_Server, y_Server, alpha=0.3, color='b', label='server')
plt.scatter(x_Client, y_Client, alpha=0.3, color='r', label='client')



#legend
leg = plt.legend()

for lh in leg.legendHandles: 
    lh.set_alpha(1)



plt.show()

