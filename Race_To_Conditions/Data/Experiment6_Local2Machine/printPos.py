import matplotlib.pyplot as plt
import pandas as pd

df_server = pd.read_csv('SphereServer.csv', delimiter=';', decimal=",")
df_client = pd.read_csv('SphereClient.csv', delimiter=';', decimal=".")



def plot(df, inputColor, inputLabel):
    x = df['ActualTime']
    y = df['Position']
    
    x.pop(0)
    y.pop(0)

    plt.scatter(x, y, alpha=0.1, color=inputColor, label=inputLabel)



plot(df_server, 'b', 'server')
plot(df_client, 'r', 'client')




#legend
leg = plt.legend()

for lh in leg.legendHandles: 
    lh.set_alpha(1)



plt.show()

