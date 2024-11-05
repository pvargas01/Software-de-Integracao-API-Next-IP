import 'package:flutter/material.dart';
import 'package:myapp2/chat_page.dart';

void main()=>runApp(const myapp2());

class myapp2 extends StatelessWidget {
  const myapp2({super.key});

  @override
  Widget build(BuildContext context) {
    return const MaterialApp(
      debugShowCheckedModeBanner: false,
      home: ChatPage(),
    );
  }
}