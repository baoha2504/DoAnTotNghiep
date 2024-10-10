import sqlite3
from db.settings import settings

# Hàm select dữ liệu từ bảng account theo username và password
def select_all_account():
    conn = sqlite3.connect(settings.DB_PATH)
    cursor = conn.cursor()
    cursor.execute('SELECT * FROM account')

    rows = cursor.fetchall()
    column_names = [description[0] for description in cursor.description]

    result = [dict(zip(column_names, row)) for row in rows]

    conn.close()
    return result

def select_account_by_username_password(username, password):
    conn = sqlite3.connect(settings.DB_PATH)
    cursor = conn.cursor()
    cursor.execute('''
        SELECT * FROM account
        WHERE username = ? AND password = ?
    ''', (username, password))
    result = cursor.fetchone()  # Lấy kết quả đầu tiên (vì username và password thường là duy nhất)
    conn.close()
    return result

def select_all_login_history():
    conn = sqlite3.connect(settings.DB_PATH)
    cursor = conn.cursor()
    cursor.execute('''
        SELECT * FROM login_history
        ORDER BY date_time DESC
        LIMIT 1000
    ''')
    results = cursor.fetchall()  # Lấy tất cả các kết quả thỏa mãn
    conn.close()
    return results

# Hàm select dữ liệu từ bảng login_history theo device_serial
def select_login_history_by_device_serial(device_serial):
    conn = sqlite3.connect(settings.DB_PATH)
    cursor = conn.cursor()
    cursor.execute('''
        SELECT * FROM login_history
        WHERE device_serial = ?
    ''', (device_serial,))
    results = cursor.fetchall()  # Lấy tất cả các kết quả thỏa mãn
    conn.close()
    return results

def select_login_history_by_account_id(account_id):
    conn = sqlite3.connect(settings.DB_PATH)
    cursor = conn.cursor()
    cursor.execute('''
        SELECT * FROM login_history
        WHERE account_id = ?
        ORDER BY date_time DESC
    ''', (account_id,))
    
    rows = cursor.fetchall()
    column_names = [description[0] for description in cursor.description]

    result = [dict(zip(column_names, row)) for row in rows]

    conn.close()
    return result

# Hàm select dữ liệu từ bảng count_coin theo account_id
def select_count_coin_by_account_id(account_id):
    conn = sqlite3.connect(settings.DB_PATH)
    cursor = conn.cursor()
    cursor.execute('''
        SELECT * FROM count_coin
        WHERE account_id = ?
    ''', (account_id,))
    results = cursor.fetchall()  # Lấy tất cả các kết quả thỏa mãn
    conn.close()
    return results

# Ví dụ sử dụng
# account = select_account_by_username_password('johndoe', 'password123')
# if account:
#     print("Account found:", account)
# else:
#     print("Account not found.")
    
# login_histories = select_login_history_by_device_serial('123ABC')
# if login_histories:
#     print("Login history found:", login_histories)
# else:
#     print("No login history found for the given device serial.")

# count_coins = select_count_coin_by_account_id(1)
# if count_coins:
#     print("Count coin records found:", count_coins)
# else:
#     print("No count coin records found for the given account_id.")