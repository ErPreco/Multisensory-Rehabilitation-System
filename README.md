# Unity-Teensy Protocol

`XY,Z:v`

- `X` : destination (`T` for Teensy, `U` for Unity)

- `Y,Z` : topic and possible sub-topic

    Unity topics and sub-topics

    - `C` : configuration

        - `AWR` : analog write resolution
    
    - `S` : sensor

        - `FLEX` : bend index of flex sensor between 0 (flat) and 1 (90° bent)
    
    Teensy topics and sub-topics

    - `H` : haptic

        - `ERM` : intensity value of analog write for ERM actuator

- `v` : value
