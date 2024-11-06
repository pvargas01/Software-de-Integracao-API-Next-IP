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
      appBar: AppBar(title: Text("Rota Oeste")),
      body: _pages[selected_index],
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: selected_index,
        onTap: _navigateBottomBar,
        items: const [
          
          BottomNavigationBarItem(
              icon: Icon(Icons.person),
              label: "Usuário"
          ),

          
          BottomNavigationBarItem(
              icon: Icon(Icons.chat),
              label: "Chats"
          ),

          
          BottomNavigationBarItem(
              icon: Icon(Icons.contacts),
              label: "Contatos"
          )
          
        ],
      ),
    );
  }
}