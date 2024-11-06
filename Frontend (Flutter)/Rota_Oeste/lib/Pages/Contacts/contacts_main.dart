import 'package:flutter/material.dart';

class Contacts extends StatelessWidget {
  const Contacts({super.key});

  @override
  Widget build(BuildContext context) {

    return Scaffold(
        appBar: AppBar(
        centerTitle: true,
        title: const Text(
          'Contatos',
          style: TextStyle(fontSize: 30),
        ),
      ),
    );
  }
}
