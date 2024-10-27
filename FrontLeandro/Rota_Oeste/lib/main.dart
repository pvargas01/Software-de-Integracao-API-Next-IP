import 'package:flutter/material.dart';
import 'package:rota_oeste/Pages/Auth_pages/login_page.dart';
import 'package:rota_oeste/Pages/main_page.dart';
import 'package:rota_oeste/Pages/user_profile.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: MainPage(),
    );
  }
}