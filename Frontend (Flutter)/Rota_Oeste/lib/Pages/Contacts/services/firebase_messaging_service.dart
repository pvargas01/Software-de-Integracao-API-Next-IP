// lib/services/firebase_messaging_service.dart

import 'package:firebase_messaging/firebase_messaging.dart';

class FirebaseMessagingService {
  final FirebaseMessaging _messaging = FirebaseMessaging.instance;

  Future<void> setupFirebaseMessaging() async {
    // Solicitar permissões para notificações
    NotificationSettings settings = await _messaging.requestPermission();

    if (settings.authorizationStatus == AuthorizationStatus.authorized) {
      print('User granted permission');
    } else {
      print('User declined permission');
    }

    // Manipular mensagens recebidas
    FirebaseMessaging.onMessage.listen((RemoteMessage message) {
      RemoteNotification? notification = message.notification;
      if (notification != null) {
        print('Message received: ${notification.title} - ${notification.body}');
      }
    });

    FirebaseMessaging.onMessageOpenedApp.listen((RemoteMessage message) {
      print('A message was opened: ${message.messageId}');
    });
  }

  Future<String?> getToken() async {
    return await _messaging.getToken();
  }
}
