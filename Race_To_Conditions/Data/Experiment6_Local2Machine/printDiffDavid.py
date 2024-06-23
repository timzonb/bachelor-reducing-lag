import matplotlib.pyplot as plt
import pandas as pd

df_server = pd.read_csv('SphereServer.csv', delimiter=';', decimal=",")
df_client = pd.read_csv('SphereClient.csv', delimiter=';', decimal=".")



def plot(df, inputColor, inputLabel):
    x = df['ActualTime']
    y = df['Position']
    
    x.pop(0)
    y.pop(0)

    plt.scatter(x, y, alpha=0.3, color=inputColor, label=inputLabel)


df_difference = df_server - df_client

print(df_difference)


plot(df_server, 'b', 'server')
plot(df_client, 'r', 'client')

x = df_server['ActualTime']
y = df_difference['Position']

#x.pop(0)
y.pop(0)

print(len(x))
print(len(y))

plt.scatter(x, y, alpha=0.3, color='m', label='difference')

plt.axhline(y=0.0, color='k', linestyle='-')


#legend
leg = plt.legend()

for lh in leg.legendHandles: 
    lh.set_alpha(1)



plt.show()