import json

def read_json_file(pathFile):
    try:
        with open(pathFile, 'r', encoding='utf-8') as file:
            data = json.load(file)
            print(json.dumps(data, indent=4))
    except FileNotFoundError:
        print(f"File not found: {pathFile}")
    except json.JSONDecodeError:
        print(f"Error decoding JSON from the file: {pathFile}")