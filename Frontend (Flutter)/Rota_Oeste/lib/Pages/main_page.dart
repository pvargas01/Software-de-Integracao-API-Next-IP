import 'package:flutter/material.dart';
import 'package:rota_oeste/Pages/Chat_dir/chat_main.dart';
import 'package:rota_oeste/Pages/Contacts/contacts_main.dart';
import 'package:rota_oeste/Pages/user_profile.dart';

class MainPage extends StatefulWidget {
  MainPage({super.key});

  @override
  State<MainPage> createState() => _MainPageState();
}

class _MainPageState extends State<MainPage> {
  int selected_index = 0;

  void _navigateBottomBar(int index) {
    setState(() {
      selected_index = index;
    });
  }

  final List<Widget> _pages = [
    UserProfile(),
    ChatMain(),
    Contacts()
  ];

  @override
  Widget build(BuildContext context) {

    return Scaffold(
      appBar: AppBar(title: Text("Main Page")),
      body: _pages[selected_index],
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: selected_index,
        onTap: _navigateBottomBar,
        items: const [
          // Usuário
          BottomNavigationBarItem(
              icon: Icon(Icons.person),
              label: "Usuário"
          ),

          // Chat
          BottomNavigationBarItem(
              icon: Icon(Icons.chat),
              label: "Chats"
          ),

          // Contatos
          BottomNavigationBarItem(
              icon: Icon(Icons.contacts),
              label: "Contatos"
          )
          //
        ],
      ),
    );
  }
}