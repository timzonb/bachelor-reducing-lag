import matplotlib.pyplot as plt
import pandas as pd

df_server = pd.read_csv('SphereOne.csv', delimiter=';', decimal=",")
df_client = pd.read_csv('SphereTwo.csv', delimiter=';', decimal=",")



def plot(df, inputColor, inputLabel):
    x = df['Time']
    y = df['Position']
    
    x.pop(0)
    y.pop(0)

    plt.scatter(x, y, alpha=0.3, color=inputColor, label=inputLabel)


df_difference = df_server - df_client
x = df_server['Time']
y = df_difference['Position']
plt.scatter(x, y, alpha=0.3, color='m', label='difference')

plot(df_server, 'b', 'Sphere One')
plot(df_client, 'r', 'Sphere Two')


plt.axhline(y=0.0, color='k', linestyle='-')

leg = plt.legend(prop={'size': 15})

for lh in leg.legendHandles: 
    lh.set_alpha(1)


plt.show()



