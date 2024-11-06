import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

class UserProfile extends StatefulWidget {
  @override
  _UserProfileState createState() => _UserProfileState();
}

class _UserProfileState extends State<UserProfile> {
  final storage = FlutterSecureStorage();
  String userName = '';
  String userEmail = '';
  String userUsername = '';

  @override
  void initState() {
    super.initState();
    logar();
  }

  logar() async {
    var url = Uri.parse('http://localhost:5000/api/users/me');
    String? token = await storage.read(key: 'token');
    try {
      var response = await http.get(
        url,
        headers: {
          'token': '$token',
          'Access-Control-Allow-Origin': '*',
        },
      );

      if (response.statusCode == 200) {
        var data = jsonDecode(response.body);
        setState(() {
          userName = data['name'];
          userEmail = data['email'];
          userUsername = data['username'];
        });
      } else {
        print('Erro ao obter dados do usuário: ${response.statusCode}');
      }
    } catch (e) {
      print('Erro de conexão: $e');
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        centerTitle: true,
        title: const Text(
          'Usuário',
          style: TextStyle(fontSize: 30),
        ),
      ),
      body: Padding(
        padding: EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            Text(
              'Nome:',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            SizedBox(height: 8),
            Text(
              userName,
              style: TextStyle(fontSize: 16),
            ),
            Divider(height: 20, thickness: 1),

            Text(
              'Email:',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            SizedBox(height: 8),
            Text(
              userEmail,
              style: TextStyle(fontSize: 16),
            ),
            Divider(height: 20, thickness: 1),

            Text(
              'Username:',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            SizedBox(height: 8),
            Text(
              userUsername,
              style: TextStyle(fontSize: 16),
            ),
            Divider(height: 20, thickness: 1),
          ],
        ),
      ),
    );
  }
}