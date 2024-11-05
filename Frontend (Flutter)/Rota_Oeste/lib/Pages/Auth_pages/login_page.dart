import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

class LoginPage2 extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        appBar: AppBar(
          title: Text("Login"),
        ),
        body: LoginForm(),
      ),
    );
  }
}

class LoginForm extends StatefulWidget {
  @override
  _LoginFormState createState() => _LoginFormState();
}

class _LoginFormState extends State<LoginForm> {
  final _formKey = GlobalKey<FormState>();

  String? _validateEmail(String? value) {
    if (value == null || value.isEmpty) {
      return 'Email é obrigatório';
    }
    final emailRegex = RegExp(r'^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$');
    if (!emailRegex.hasMatch(value)) {
      return 'Insira um email válido';
    }
    return null;
  }

  String? _validatePassword(String? value) {
    if (value == null || value.isEmpty) {
      return 'Senha é obrigatória';
    } else if (value.length < 8) {
      return 'A senha deve ter pelo menos 8 caracteres';
    }
    return null;
  }

  void _submitForm() {
    logar();
    if (_formKey.currentState!.validate()) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Login bem-sucedido!')),
      );
    }
  }

  logar() async {
    var url = Uri.parse('');
    var response = await http.post(
        url,
        body:{
          'username': _emailController.text,
          'password': _senhaController.text
        }
    );


  }

  final _emailController = TextEditingController();
  final _senhaController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: EdgeInsets.all(16.0),
      child: Form(
        key: _formKey,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            TextFormField(
              decoration: InputDecoration(labelText: 'Email'),
              keyboardType: TextInputType.emailAddress,
              validator: _validateEmail,
              controller: _emailController,
            ),
            TextFormField(
              decoration: InputDecoration(labelText: 'Senha'),
              obscureText: true,
              validator: _validatePassword,
              controller: _senhaController,
            ),
            SizedBox(height: 20),
            ElevatedButton(
              onPressed: _submitForm,
              child: Text('Login'),
            ),
          ],
        ),
      ),
    );
  }
}