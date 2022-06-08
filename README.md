# ChatManagerUtility

This plugin basically allows you globa, local, team, and private chat. Local is bounded by a 9feet magnitude check (Height is not calculated), and is enabled by the host.
If a channel is enabled, consumers (players) can unsubscribe from the stream (chat) through a chatlimit command (run to enable/disable). Below is an example config.

![ChatManagerUtility ISSUES](https://img.shields.io/github/issues/Undid-Iridium/ChatManagerUtility)
![ChatManagerUtility FORKS](https://img.shields.io/github/forks/Undid-Iridium/ChatManagerUtility)
![ChatManagerUtility LICENSE](https://img.shields.io/github/license/Undid-Iridium/ChatManagerUtility)


![ChatManagerUtility LATEST](https://img.shields.io/github/v/release/Undid-Iridium/ChatManagerUtility?include_prereleases&style=flat-square)
![ChatManagerUtility LINES](https://img.shields.io/tokei/lines/github/Undid-Iridium/ChatManagerUtility)
![ChatManagerUtility DOWNLOADS](https://img.shields.io/github/downloads/Undid-Iridium/ChatManagerUtility/total?style=flat-square)



## REQUIREMENTS
* Exiled: V5.1.3 
* SCP:SL Server: V11.2
* 
```
chat_manager_utility:
chat_manager_utility:
# Whether to enabled or disable plugin
  is_enabled: true
  # Whether to enabled/disable debug
  is_debug_enabled: true
  # How long to sleep on every iteration before consuming more messages (In seconds).
  sleep_time: 1
  # Amount of characters per line to show
  character_limit: 64
  # Amount of lines to show
  display_limit: 15
  # How long to show the messages
  display_time_limit: 15
  # Where to place text (Always on bottom)
  text_placement: Left
  # Chat colors instance
  associated_chat_colors:
  # Global chat color - Use hex to assign the color.
    global_chat_color: <color=#85C7F2>
    # Local chat color - Use hex to assign the color.
    local_chat_color: <color=#85C7F2>
    # Private chat color - Use hex to assign the color.
    private_chat_color: <color=#ADD7F6>
    # Team chat color - Use hex to assign the color.
    team_chat_color: <color=#3B28CC>
  # Size of the text to show
  size_of_hint_text: <size=50%>
  # Whether to allow type of messages, if not specified then it will be ignored and commands for it rejected.
  msg_types_allowed:
  - GLOBAL
  - LOCAL
  - PRIVATE
  - TEAM
  # Whether to send the messages to console
  send_to_console: false
  # Whether to send the messages to hint system
  send_to_hint_system: true
  ```
