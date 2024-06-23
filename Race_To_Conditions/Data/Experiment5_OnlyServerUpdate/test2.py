import matplotlib.pyplot as plt
import pandas as pd

x = [5, 10, 20, 4, 9, 6, 40]
y = [1, 2, 3, 4, 5, 6, 7]

plt.scatter(x, y)

plt.gcf().autofmt_xdate()

plt.show()