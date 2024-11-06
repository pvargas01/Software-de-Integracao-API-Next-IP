import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:rota_oeste/Pages/Auth_pages/login_page.dart';
import 'package:rota_oeste/Pages/Auth_pages/register_page.dart';
import 'package:rota_oeste/Pages/main_page.dart';
import 'package:http/http.dart' as http;
import 'package:rota_oeste/Pages/user_profile.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatefulWidget {
  const MyApp({super.key});

  @override
  _MyAppState createState() => _MyAppState();
}

class _MyAppState extends State<MyApp> {
  final storage = FlutterSecureStorage();
  bool autenticado = false;
  bool verificando = true;

  @override
  void initState() {
    super.initState();
    verificar();
  }

  Future<void> verificar() async {
    String? token = await storage.read(key: 'token');

    if (token == null) {
      setState(() {
        autenticado = false;
        verificando = false;
      });
      return;
    }

    final url = Uri.parse(''); 
    final response = await http.post(
      url,
      headers: {
        'Authorization': 'Bearer $token',
        'Content-Type': 'application/json',
      },
    );

    setState(() {
      autenticado = response.statusCode == 200;
      verificando = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    if (verificando) {
      return const MaterialApp(
        home: Scaffold(
          body: Center(child: CircularProgressIndicator()),
        ),
      );
    }

    return MaterialApp(
      debugShowCheckedModeBanner: false,
      home: autenticado ? MainPage() : RegisterPage(),
    );
  }
}