def read_file_txt(path):
    try:
        with open(path, 'r', encoding='utf-8') as file:
            content = file.read()
        return content
    except IOError as e:
        print(f"An IOError occurred: {e}")
        return ''
