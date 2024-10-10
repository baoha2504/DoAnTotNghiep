package com.example.coinmining

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.input.VisualTransformation
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.Alignment
import androidx.navigation.NavHostController
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.example.coinmining.ui.theme.CoinMiningTheme
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.sp
import androidx.compose.foundation.shape.RoundedCornerShape
import android.content.Intent
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.graphics.painter.Painter
import androidx.compose.ui.res.painterResource
import androidx.compose.foundation.Image
import androidx.lifecycle.lifecycleScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody.Companion.toRequestBody
import okhttp3.Response
import org.json.JSONObject
import java.io.IOException
import android.content.Context
import androidx.appcompat.app.AppCompatActivity
import android.os.Build
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.launch
import android.widget.Toast
import android.provider.Settings



class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            CoinMiningTheme {
                val navController = rememberNavController()
                NavHost(navController = navController, startDestination = "login") {
                    composable("login") {
                        LoginScreen(navController)
                    }
                    composable("checkPIN") {
                        CheckPIN()
                    }
                }
            }
        }
    }
}

// Hàm lưu account_id vào SharedPreferences
private fun saveAccountId(context: Context, accountId: Int) {
    val sharedPreferences = context.getSharedPreferences("LoginData", Context.MODE_PRIVATE)
    val editor = sharedPreferences.edit()
    editor.putInt("account_id", accountId)
    editor.apply()
}

// Hàm dùng để gửi dữ liệu đăng nhập
private suspend fun loginUser(context: Context, username: String, password: String): String {
    val client = OkHttpClient()

    val json = JSONObject().apply {
        put("username", username)
        put("password", password)
    }

    val mediaType = "application/json; charset=utf-8".toMediaTypeOrNull()
    val body = json.toString().toRequestBody(mediaType)

    val request = Request.Builder()
        .url("http://192.168.43.107:5001/login")
        .post(body)
        .build()

    return try {
        val response: Response = withContext(Dispatchers.IO) {
            client.newCall(request).execute()
        }

        if (response.isSuccessful) {
            val responseBody = response.body?.string()
            val jsonResponse = JSONObject(responseBody ?: "")
            val message = jsonResponse.getString("message")
            if (message == "Login successful") {
                val accountId = jsonResponse.getInt("account_id")
                saveAccountId(context, accountId)
                "Đăng nhập thành công!"
            } else {
                "Sai tài khoản hoặc mật khẩu!"
            }
        } else {
            "Sai tài khoản hoặc mật khẩu!"
        }
    } catch (e: IOException) {
        "Lỗi mạng: ${e.message}"
    }
}

// Hàm dùng để gửi dữ liệu đăng nhập bằng PIN
private suspend fun loginWithPin(context: Context, pincode: String): String {
    val client = OkHttpClient()

    // Lấy thông tin thiết bị Android
    val deviceSerial = Build.SERIAL // Hoặc Build.getSerial() tùy thuộc vào API
    val deviceName = Build.MODEL

    // Lấy account_id từ SharedPreferences
    val sharedPreferences = context.getSharedPreferences("LoginData", Context.MODE_PRIVATE)
    val accountId = sharedPreferences.getInt("account_id", -1)

    if (accountId == -1) {
        return "Không tìm thấy account_id, vui lòng đăng nhập lại."
    }

    // Tạo JSON dữ liệu cần gửi
    val json = JSONObject().apply {
        put("device_serial", deviceSerial)
        put("device_name", deviceName)
        put("pincode", pincode)
        put("account_id", accountId)
    }

    val mediaType = "application/json; charset=utf-8".toMediaTypeOrNull()
    val body = json.toString().toRequestBody(mediaType)

    // Tạo request gửi đến API loginwithpin
    val request = Request.Builder()
        .url("http://192.168.43.107:5001/loginwithpin")
        .post(body)
        .build()

    return try {
        // Thực hiện request trên thread I/O
        val response: Response = withContext(Dispatchers.IO) {
            client.newCall(request).execute()
        }

        // Kiểm tra kết quả
        if (response.isSuccessful) {
            val responseBody = response.body?.string()
            val jsonResponse = JSONObject(responseBody ?: "")
            val message = jsonResponse.getString("message")
            if (message == "Login pin successful") {
                "Đăng nhập thành công!"
            } else {
                "Sai mã PIN!"
            }
        } else {
            "Lỗi đăng nhập với PIN!"
        }
    } catch (e: IOException) {
        "Lỗi mạng: ${e.message}"
    }
}


@Composable
fun LoginScreen(navController: NavHostController) {
    var username by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    var loginResult by remember { mutableStateOf("") }
    val context = LocalContext.current
    val scope = rememberCoroutineScope()

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp),
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        // Thêm ảnh
        val image: Painter = painterResource(id = R.drawable.dollar512)
        Image(
            painter = image,
            contentDescription = "Logo Image",
            modifier = Modifier
                .size(120.dp) // Kích thước ảnh
                .padding(bottom = 24.dp) // Khoảng cách dưới ảnh
        )

        Text(text = "MTA Coin Forensic", style = MaterialTheme.typography.headlineLarge)

        Spacer(modifier = Modifier.height(24.dp))

        OutlinedTextField(
            value = username,
            onValueChange = { username = it },
            label = { Text("Tài khoản") },
            modifier = Modifier.fillMaxWidth(),
            shape = RoundedCornerShape(10.dp)
        )

        Spacer(modifier = Modifier.height(16.dp))

        OutlinedTextField(
            value = password,
            onValueChange = { password = it },
            label = { Text("Mật khẩu") },
            modifier = Modifier.fillMaxWidth(),
            shape = RoundedCornerShape(10.dp),
            visualTransformation = PasswordVisualTransformation(),
            keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Password)
        )

        Spacer(modifier = Modifier.height(24.dp))

        val scope = rememberCoroutineScope()

        Button(
            onClick = {
//                scope.launch {
//                    val result = loginUser(context, username, password)
//                    loginResult = result
//                    if (result == "Đăng nhập thành công!") {
//                        navController.navigate("checkPIN")
//                    }
//                }
                scope.launch(Dispatchers.IO) {
                    val result = loginUser(context, username, password)
                    withContext(Dispatchers.Main) {
                        loginResult = result
                        if (result == "Đăng nhập thành công!") {
                            navController.navigate("checkPIN")
                        }
                    }
                }

            },
            modifier = Modifier.fillMaxWidth()
        ) {
            Text(text = "Đăng nhập")
        }

        Spacer(modifier = Modifier.height(16.dp))

        // Hiển thị kết quả đăng nhập
        if (loginResult.isNotEmpty()) {
            Text(text = loginResult)
        }

        Spacer(modifier = Modifier.height(16.dp))

        TextButton(onClick = { /* Handle forgot password logic */ }) {
            Text(text = "Quên mật khẩu?")
        }
    }
}

@Composable
fun CheckPIN() {
    // State to hold the PIN input value
    var pin by remember { mutableStateOf("") }

    // Get the current context
    val context = LocalContext.current

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp),
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        // Thêm ảnh
        val image: Painter = painterResource(id = R.drawable.dollar512)
        Image(
            painter = image,
            contentDescription = "Logo Image",
            modifier = Modifier
                .size(120.dp) // Kích thước ảnh
                .padding(bottom = 50.dp) // Khoảng cách dưới ảnh
        )

        Text(
            text = "Nhập mã PIN",
            style = MaterialTheme.typography.headlineMedium,
            modifier = Modifier.padding(bottom = 16.dp)
        )

        // Text field for PIN entry
        TextField(
            value = pin,
            onValueChange = { newPin ->
                // Update the PIN value if it's a digit and not more than 6 characters
                if (newPin.all { it.isDigit() } && newPin.length <= 6) {
                    pin = newPin
                }
            },
            label = { Text("Mã PIN") },
            keyboardOptions = KeyboardOptions.Default.copy(keyboardType = KeyboardType.Number),
            singleLine = true,
            textStyle = TextStyle(
                textAlign = TextAlign.Center,
                fontSize = 18.sp // Increase the font size as needed
            ), // Center the text inside the TextField
            modifier = Modifier
                .fillMaxWidth()
                .padding(horizontal = 16.dp) // Add horizontal padding to ensure it doesn't stretch too much
        )

        Spacer(modifier = Modifier.height(16.dp))

        // Button to confirm the PIN entry
        Button(
            onClick = {
                // Kiểm tra độ dài mã PIN
                if (pin.length == 6) {
                    // Gọi hàm loginWithPin để xác thực mã PIN với API
                    CoroutineScope(Dispatchers.IO).launch {
                        val result = loginWithPin(context, pin)

                        // Kiểm tra kết quả từ API
                        if (result == "Đăng nhập thành công!") {
                            // Tạo Intent để chuyển đến HomeActivity
                            val intent = Intent(context, HomeActivity::class.java)
                            // Bắt đầu HomeActivity
                            context.startActivity(intent)
                        } else {
                            // Hiển thị thông báo lỗi
                            Toast.makeText(context, result, Toast.LENGTH_LONG).show()
                        }
                    }
                } else {
                    // Hiển thị thông báo nếu mã PIN không hợp lệ
                    Toast.makeText(context, "Mã PIN phải là 6 chữ số", Toast.LENGTH_SHORT).show()
                }
            },
            enabled = pin.length == 6
        ) {
            Text("Xác thực")
        }
    }
}


@Preview(showBackground = true)
@Composable
fun LoginScreenPreview() {
    CoinMiningTheme {
        LoginScreen(rememberNavController())
    }
}

@Preview(showBackground = true)
@Composable
fun CheckPINPreview() {
    CoinMiningTheme {
        CheckPIN()
    }
}
