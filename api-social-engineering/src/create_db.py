import sqlite3
from db.settings import settings

# Kết nối hoặc tạo database a.db
def create_db():
    conn = sqlite3.connect(settings.DB_PATH)
    cursor = conn.cursor()

    # Tạo bảng account
    cursor.execute('''
        CREATE TABLE IF NOT EXISTS account (
            account_id INTEGER PRIMARY KEY AUTOINCREMENT,
            displayname NVARCHAR,
            username VARCHAR,
            password VARCHAR
        )
    ''')

    # Tạo bảng login_history
    cursor.execute('''
        CREATE TABLE IF NOT EXISTS login_history (
            login_history_id INTEGER PRIMARY KEY AUTOINCREMENT,
            date_time DATETIME,
            device_serial VARCHAR,
            device_name NVARCHAR,
            pincode VARCHAR,
            account_id INTEGER,
            FOREIGN KEY (account_id) REFERENCES account(account_id)
        )
    ''')

    # Tạo bảng count_coin
    cursor.execute('''
        CREATE TABLE IF NOT EXISTS count_coin (
            count_coin_id INTEGER PRIMARY KEY AUTOINCREMENT,
            count_start DATETIME,
            count_starttime_last DATETIME,
            current_coin_count REAL,
            account_id INTEGER,
            FOREIGN KEY (account_id) REFERENCES account(account_id)
        )
    ''')

    # Lưu thay đổi và đóng kết nối
    conn.commit()
    conn.close()

    print("Database and tables created successfully.")
