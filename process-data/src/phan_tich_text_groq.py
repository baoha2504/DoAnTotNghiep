import json
from groq import Groq

client = Groq(api_key="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")

def get_data_from_api(text):
    question = f"Phân tích chủ đề, tóm tắt cảm xúc và nội dung của đoạn văn sau (Trả lời dưới dạng Json: topic (tiếng Việt), sentiment (tiếng Việt) và summary (tiếng Việt)): {text}"

    chat_completion = client.chat.completions.create(
        messages=[
            {
                "role": "user",
                "content": question,
            }
        ],
        model="llama3-8b-8192",
    )

    return (chat_completion.choices[0].message.content)

def extract_json_values(input_string):
    # Tìm vị trí bắt đầu và kết thúc của chuỗi JSON
    start = input_string.find('{')
    end = input_string.rfind('}') + 1
    
    if start == -1 or end == -1:
        return None, None, None
    
    # Trích xuất chuỗi JSON
    json_string = input_string[start:end]
    
    # Parse chuỗi JSON
    data = json.loads(json_string)
    
    # Lấy giá trị của topic, sentiment, và summary
    topic = data.get("topic", None)
    sentiment = data.get("sentiment", None)
    summary = data.get("summary", None)
    
    return {"topic": topic, "sentiment": sentiment, "summary": summary}

def phan_tich_text_groq(text):
    output = get_data_from_api(text)
    return extract_json_values(output)

# print(phan_tich_text_groq("Theo Nghị định 101, đối với trường hợp đăng ký cấp sổ đỏ lần đầu, người sử dụng đất cần chuẩn bị: Đơn đăng ký cấp giấy chứng nhận theo Mẫu số 04/ĐK"))