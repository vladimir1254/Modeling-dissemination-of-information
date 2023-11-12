import sys
import numpy as np
from re import X
from scipy.integrate import odeint
import json
from scipy.integrate import solve_ivp



# Определение функции, представляющей систему дифференциальных уравнений

def system(t,y,a, b, N1, N2, d, y_value,f1, f2):
    dX1dt = (f1(y[0]+y[1])*(y[2]+y[0])/N1-y[0])* (a + b * (y[0] + y[1])) - y_value * y[0]
    dX2dt = (f2(y[0]+y[1])*(y[3]+y[1])/N2-y[1]) * b * (y[0] +y[1]) - y_value * y[1]
    dx1dt=   (a + b * (y[0] + y[1])) * (N1 - y[2] -f1(y[0]+y[1])*(y[2]+y[0])/N1) - d * y[2] + y_value * y[0]
    dx2dt = b * (y[0] + y[1]) * (N2 - y[3] - f2(y[0] + y[1])*(y[3]+y[1])/N2) - d * y[3] + y_value * y[1]
    return [dX1dt,dX2dt,dx1dt,dx2dt]#[dX1dt, dX2dt, dx1dt, dx2dt]




# 1000 1000 1 1 1 1 x x


def process_data(input_data,inp_f1,inp_f2):
    # Задание начальных значений переменных
    initial_conditions = [0,0,0,0]

    # Задание параметров
    N1 = input_data[0]
    N2 = input_data[1]
    a = input_data[2]
    b = input_data[3]
    y_value = input_data[5] 
    d = input_data[4]


    # Задание массива времени
    t = np.linspace(0, 100, 100)  # от 0 до 100 секунд, 1000 точек

    # Определение функций f1 и f2
    def f1(x):
        return min(eval(str(inp_f1)),N1)

    def f2(x):
        # Ваши вычисления для f2
        return min(eval(str(inp_f2)),N2)


    method = 'lsoda'
    atol = 1e-8
    rtol = 1e-8
    np.random.seed(42)
    # Решение системы дифференциальных уравнений
    t_span = (0, 1)  # Задайте t_start и t_end
    t_eval =  np.linspace(0, 1, 1000)  # Предположим, что t - это ваш массив временных точек

    # Решение с использованием solve_ivp
    solution = solve_ivp(system, t_span=t_span,method='Radau', y0=initial_conditions, args=(a, b, N1, N2, d,y_value, f1, f2),t_eval = t_eval
           ,rtol=1e-1, atol=1e-1)
   # print(solution)
    solution = solution.y
    # solution содержит значения переменных системы уравнений на заданных точках времени t
    X1, X2, x1, x2= [round(item, 3)if item <= N1 or item==np.inf or item==np.nan else N1 for item in  solution[0]],\
    [round(item, 3)if item <= N2 or item==np.inf or item==np.nan else N2 for item in solution[ 1]],\
    [round(item, 3)if item <= N1 or item==np.inf or item==np.nan else N1 for item in solution[ 2]],\
    [round(item, 3)if item <= N2 or item==np.inf or item==np.nan else N2 for item in solution[3]]
    return X1,X2,x1,x2,input_data

# Считываем входные данные из стандартного ввода
#input_data = [float(x.replace(',','.')) for x in input().split()]
input_data = [float(x.replace(',', '.')) if index < 6 else x for index, x in enumerate(input().split())]
f1 = input_data[-2]
f2 = input_data[-1]
input_data = input_data[:6]
# Обрабатываем данные
try:
    result = process_data(input_data,f1,f2)
    N1 = result[4][0]
    N2 = result[4][1]
    result =[[round(item, 3)if item >= 0 else 0 for item in sublist] for sublist in result]

    result =[[round(item, 3)if item <= 10**20 else 10**20 for item in sublist] for sublist in result]
    #print(result)
    result_json = json.dumps(list(result))
    # Возвращаем результат в стандартный вывод
    print(result_json)

except Exception as ex:
    print(ex)
    print('Error')



# 18000 5000 1 1 
