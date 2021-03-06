﻿using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;

namespace Typing {
    public partial class MainWindow : Window {
        private MediaPlayer[] players;
        private int index;
        private Dictionary<int, bool> pressed = new Dictionary<int, bool>();

        public MainWindow() {
            InitializeComponent();
            Hook.KeyboardHook.KeyDown += KeyboardHook_KeyDown;
            Hook.KeyboardHook.KeyUp += KeyboardHook_KeyUp;
            Hook.KeyboardHook.HookStart();

            players = new MediaPlayer[5];
            var audioPath = new Uri($"file:///{Path.Combine(Directory.GetCurrentDirectory(), "Sound.wav")}");
            for (var i = 0; i < players.Length; i++) {
                players[i] = new MediaPlayer();
                players[i].Open(audioPath);
            }
        }

        ~MainWindow() {
            Hook.KeyboardHook.HookEnd();
        }

        private bool KeyboardHook_KeyDown(int vkCode) {
            if (!pressed.ContainsKey(vkCode)) {
                pressed[vkCode] = false;
            }
            if (!pressed[vkCode]) {
                Play();
                pressed[vkCode] = true;
            }
            return true;
        }

        private bool KeyboardHook_KeyUp(int vkCode) {
            pressed[vkCode] = false;
            return true;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                this.DragMove();
            }
            else {
                Play();
            }
        }

        private void Play() {
            players[index].Stop();
            players[index].Play();
            index = (index + 1) % players.Length;
        }
    }
}
