package com.example.getcalllogandroid

import android.Manifest
import android.content.pm.PackageManager
import android.database.Cursor
import android.os.Build
import android.os.Bundle
import android.provider.CallLog
import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.text.BasicTextField
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.core.content.ContextCompat
import com.example.getcalllogandroid.ui.theme.GetCallLogAndroidTheme
import androidx.compose.foundation.border
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.sp
import org.json.JSONArray
import org.json.JSONObject
import java.io.OutputStream
import java.net.HttpURLConnection
import java.net.URL
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import android.util.Log
import java.util.regex.Pattern

class MainActivity : ComponentActivity() {
    private var callLogs = mutableStateListOf<String>()
    private var serverIp by mutableStateOf("")
    private var isIpValid by mutableStateOf(false)

    private val requestPermissionLauncher =
        registerForActivityResult(ActivityResultContracts.RequestPermission()) { isGranted: Boolean ->
            if (isGranted) {
                fetchCallLogs()
            } else {
                Toast.makeText(this, "Permission denied to read call logs", Toast.LENGTH_SHORT).show()
            }
        }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            GetCallLogAndroidTheme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    Column(
                        modifier = Modifier
                            .padding(innerPadding)
                            .fillMaxSize()
                            .padding(16.dp)
                    ) {
                        // Header Text
                        Text(
                            text = "GET CALL LOGS ANDROID",
                            style = MaterialTheme.typography.headlineMedium.copy(
                                fontSize = 24.sp,
                                fontWeight = FontWeight.Bold
                            ),
                            modifier = Modifier
                                .padding(bottom = 16.dp)
                                .fillMaxWidth(),
                            color = MaterialTheme.colorScheme.primary
                        )

                        Text(
                            text = "Input IP Server",
                            style = MaterialTheme.typography.headlineMedium.copy(
                                fontSize = 16.sp,
                                fontWeight = FontWeight.Medium
                            ),
                            modifier = Modifier
                                .padding(bottom = 8.dp)
                                .fillMaxWidth(),
                            color = MaterialTheme.colorScheme.onBackground
                        )

                        // TextField to enter server IP
                        BasicTextField(
                            value = serverIp,
                            onValueChange = {
                                serverIp = it
                                isIpValid = validateIp(it)
                            },
                            keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Text),
                            modifier = Modifier
                                .fillMaxWidth()
                                .border(
                                    BorderStroke(1.dp, if (isIpValid || serverIp.isEmpty()) Color.Gray else Color.Red),
                                    shape = MaterialTheme.shapes.small
                                )
                                .padding(12.dp),
                            textStyle = MaterialTheme.typography.bodyMedium.copy(color = MaterialTheme.colorScheme.onSurface)
                        )
                        if (!isIpValid && serverIp.isNotEmpty()) {
                            Text(
                                text = "Invalid IP address",
                                color = Color.Red,
                                style = MaterialTheme.typography.bodySmall,
                                modifier = Modifier.padding(top = 4.dp)
                            )
                        }

                        Spacer(modifier = Modifier.height(16.dp))

                        // Button to fetch Call Log
                        Button(
                            onClick = { getCallLogs() },
                            modifier = Modifier.fillMaxWidth(),
                            enabled = isIpValid,
                            colors = ButtonDefaults.buttonColors(
                                containerColor = MaterialTheme.colorScheme.primary,
                                contentColor = MaterialTheme.colorScheme.onPrimary
                            )
                        ) {
                            Text(text = "Get and send to server")
                        }

                        Spacer(modifier = Modifier.height(16.dp))

                        // Display the call logs
                        LazyColumn(
                            modifier = Modifier
                                .fillMaxSize()
                                .padding(top = 16.dp)
                        ) {
                            items(callLogs) { callLog ->
                                Card(
                                    modifier = Modifier
                                        .fillMaxWidth()
                                        .padding(vertical = 4.dp),
                                    colors = CardDefaults.cardColors(
                                        containerColor = MaterialTheme.colorScheme.surface
                                    ),
                                    elevation = CardDefaults.cardElevation(4.dp)
                                ) {
                                    Text(
                                        text = callLog,
                                        modifier = Modifier.padding(16.dp),
                                        style = MaterialTheme.typography.bodyMedium,
                                        color = MaterialTheme.colorScheme.onSurface
                                    )
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private fun getCallLogs() {
        if (ContextCompat.checkSelfPermission(this, Manifest.permission.READ_CALL_LOG) == PackageManager.PERMISSION_GRANTED) {
            fetchCallLogs()
        } else {
            requestPermissionLauncher.launch(Manifest.permission.READ_CALL_LOG)
        }
    }

    private fun fetchCallLogs() {
        val cursor: Cursor? = contentResolver.query(
            CallLog.Calls.CONTENT_URI,
            null,
            null,
            null,
            null
        )
        cursor?.use {
            var count = 1 // Biến đếm để theo dõi thứ tự
            callLogs.clear()
            val logsArray = JSONArray()
            val deviceName = "${Build.MANUFACTURER}_${Build.MODEL}"

            while (it.moveToNext()) {
                val number = it.getString(it.getColumnIndex(CallLog.Calls.NUMBER) ?: -1)
                val type = it.getString(it.getColumnIndex(CallLog.Calls.TYPE) ?: -1)
                val date = it.getString(it.getColumnIndex(CallLog.Calls.DATE) ?: -1)
                val duration = it.getString(it.getColumnIndex(CallLog.Calls.DURATION) ?: -1)

                // Add call log information to the list with order number
                callLogs.add("Row: $count, Number: $number, Type: $type, Date: $date, Duration: $duration seconds")

                // Tạo đối tượng JSON và thêm vào mảng
                val logObject = JSONObject()
                logObject.put("row", count)
                logObject.put("number", number)
                logObject.put("type", type)
                logObject.put("date", date)
                logObject.put("duration", duration)
                logObject.put("device_name", deviceName)
                logsArray.put(logObject)

                count++ // Tăng biến đếm sau mỗi lần lặp
            }
            CoroutineScope(Dispatchers.IO).launch {
                sendCallLogsToServer(logsArray.toString())
            }
        }
    }

    private fun sendCallLogsToServer(callLogsJson: String) {
        try {
            val url = URL("http://$serverIp:5001/ListenerClient")
            val conn: HttpURLConnection = url.openConnection() as HttpURLConnection
            conn.requestMethod = "POST"
            conn.setRequestProperty("Content-Type", "application/json")
            conn.doOutput = true

            val outputStream: OutputStream = conn.outputStream
            outputStream.write(callLogsJson.toByteArray())
            outputStream.flush()
            outputStream.close()

            val responseCode = conn.responseCode
            if (responseCode == HttpURLConnection.HTTP_OK) {
                runOnUiThread {
                    Toast.makeText(this, "Data sent successfully!", Toast.LENGTH_SHORT).show()
                }
            } else {
                runOnUiThread {
                    Toast.makeText(this, "Failed to send data! Response code: $responseCode", Toast.LENGTH_SHORT).show()
                }
            }
        } catch (e: Exception) {
            runOnUiThread {
                Toast.makeText(this, "Error: ${e.message}", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun validateIp(ip: String): Boolean {
        val ipPattern = Pattern.compile(
            "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\\.|$)){4}$"
        )
        return ipPattern.matcher(ip).matches()
    }
}

@Preview(showBackground = true)
@Composable
fun GreetingPreview() {
    GetCallLogAndroidTheme {
        // Preview may not display correctly if there's no actual data
        Column {
            Text("Example Call Log Item")
        }
    }
}
