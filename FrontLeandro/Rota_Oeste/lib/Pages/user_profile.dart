import 'package:flutter/material.dart';
import 'package:rota_oeste/Pages/Chat_dir/chat_main.dart';
import 'package:rota_oeste/Pages/Contacts/contacts_main.dart';

class UserProfile extends StatelessWidget {
  const UserProfile({super.key});

  @override
  Widget build(BuildContext context) {

    return Scaffold(
      appBar: AppBar(title: Text("Profile")),
    );
  }
}