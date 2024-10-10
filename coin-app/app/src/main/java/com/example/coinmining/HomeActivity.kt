package com.example.coinmining

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Menu
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.painter.Painter
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.coinmining.ui.theme.CoinMiningTheme
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import java.text.DecimalFormat
import com.example.coinmining.screens.InfoScreen
import com.example.coinmining.screens.SettingsScreen
import androidx.compose.ui.platform.LocalContext
import androidx.compose.runtime.Composable
import android.content.Context

class HomeActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            CoinMiningTheme {
                MainScreen()
            }
        }
    }
}

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun MainScreen() {
    val drawerState = rememberDrawerState(initialValue = DrawerValue.Closed)
    var selectedMenu by remember { mutableStateOf("Trang chủ") }
    val scope = rememberCoroutineScope()

    ModalNavigationDrawer(
        drawerState = drawerState,
        drawerContent = {
            ModalDrawerSheet(
                modifier = Modifier.widthIn(max = 200.dp)
            ) {
                // Thêm ảnh
                val image: Painter = painterResource(id = R.drawable.dollar512)
                Image(
                    painter = image,
                    contentDescription = "Logo Image",
                    modifier = Modifier
                        .size(80.dp)
                        .padding(start = 16.dp)
                )
                Text(
                    text = "MENU",
                    fontSize = 18.sp,
                    fontWeight = FontWeight.Bold,
                    modifier = Modifier.padding(16.dp)
                )
                Spacer(modifier = Modifier.height(16.dp))
                NavigationDrawerItem(
                    label = { Text("Trang chủ", fontSize = 14.sp) },
                    selected = selectedMenu == "Trang chủ",
                    onClick = {
                        selectedMenu = "Trang chủ"
                        scope.launch { drawerState.close() }
                    }
                )
                NavigationDrawerItem(
                    label = { Text("Thông tin", fontSize = 14.sp) },
                    selected = selectedMenu == "Thông tin",
                    onClick = {
                        selectedMenu = "Thông tin"
                        scope.launch { drawerState.close() }
                    }
                )
                NavigationDrawerItem(
                    label = { Text("Cài đặt", fontSize = 14.sp) },
                    selected = selectedMenu == "Cài đặt",
                    onClick = {
                        selectedMenu = "Cài đặt"
                        scope.launch { drawerState.close() }
                    }
                )
                NavigationDrawerItem(
                    label = { Text("Đóng ứng dụng", fontSize = 14.sp) },
                    selected = selectedMenu == "Đóng ứng dụng",
                    onClick = {
                        selectedMenu = "Đóng ứng dụng"
                        scope.launch { drawerState.close() }
                    }
                )
            }
        }
    ) {
        Scaffold(
            topBar = {
                TopAppBar(
                    title = { Text("Coin Mining App") },
                    navigationIcon = {
                        IconButton(onClick = { scope.launch { drawerState.open() } }) {
                            Icon(
                                Icons.Filled.Menu,
                                contentDescription = "Menu"
                            )
                        }
                    }
                )
            },
            modifier = Modifier.fillMaxSize()
        ) { innerPadding ->
            when (selectedMenu) {
                "Trang chủ" -> TimeCounter(modifier = Modifier.padding(innerPadding))
                "Thông tin" -> InfoScreen(modifier = Modifier.padding(innerPadding))
                "Cài đặt" -> SettingsScreen(modifier = Modifier.padding(innerPadding))
                "Đóng ứng dụng" -> {
                    // Đảm bảo context là ComponentActivity
                    val context = LocalContext.current
                    if (context is ComponentActivity) {
                        context.finishAffinity()
                    }
                }
            }
        }
    }
}

@Composable
fun TimeCounter(modifier: Modifier = Modifier) {
    var count by remember { mutableStateOf(0.0000f) }
    var isMiningStarted by remember { mutableStateOf(false) }
    val scope = rememberCoroutineScope()
    val decimalFormat = DecimalFormat("0.0000")

    Box(
        contentAlignment = Alignment.Center,
        modifier = Modifier
            .fillMaxSize()
            .background(Color(0xFFF0F0F0))
            .padding(16.dp)
    ) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.Center,
            modifier = Modifier.fillMaxSize()
        ) {
            Card(
                shape = RoundedCornerShape(16.dp),
                elevation = CardDefaults.cardElevation(defaultElevation = 8.dp),
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(16.dp)
            ) {
                Column(
                    modifier = Modifier
                        .padding(32.dp)
                        .fillMaxWidth(),
                    horizontalAlignment = Alignment.CenterHorizontally,
                    verticalArrangement = Arrangement.Center
                ) {
                    Text(
                        text = "Số coin hiện tại:",
                        fontSize = 24.sp,
                        fontWeight = FontWeight.Bold,
                        color = Color(0xFF9C27B0)
                    )
                    Spacer(modifier = Modifier.height(16.dp))
                    Text(
                        text = decimalFormat.format(count),
                        fontSize = 40.sp,
                        fontWeight = FontWeight.ExtraBold,
                        color = Color(0xFF9C27B0)
                    )
                    Spacer(modifier = Modifier.height(16.dp))
                    Text(
                        text = if (isMiningStarted) "Số coin đang tăng!" else "Nhấn nút để bắt đầu đào",
                        fontSize = 16.sp,
                        color = Color.Gray
                    )
                }
            }

            Button(
                onClick = {
                    isMiningStarted = true
                    scope.launch {
                        while (isMiningStarted) {
                            delay(1000)
                            count += 0.0001f
                        }
                    }
                },
                modifier = Modifier
                    .padding(top = 200.dp)
                    .size(width = 150.dp, height = 40.dp),
                colors = ButtonDefaults.buttonColors(containerColor = Color(0xFF9C27B0))
            ) {
                Text(text = if (isMiningStarted) "Đang đào coin" else "Bắt đầu đào", color = Color.White)
            }
        }
    }
}

@Preview(showBackground = true)
@Composable
fun MainScreenPreview() {
    CoinMiningTheme {
        MainScreen()
    }
}
