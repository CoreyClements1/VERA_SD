#include <XInput.h>

int const AXIS_X_PIN = A0;
int const AXIS_Y_PIN = A1;
const int ADC_Max = 1023;

/* Button/LED Matrix Scanning Example - 3x3 Keypad
   Code derived from Button Pad Hookup Guide Example 2
   by Byron Jacquot @ SparkFun Electronics
     https://learn.sparkfun.com/tutorials/button-pad-hookup-guide#exercise-2-monochrome-plus-buttons
*/

//////////////////////
// Config Variables //
//////////////////////
#define NUM_LED_COLS (0) // Number of LED columns (+, anode)
#define NUM_LED_ROWS (0) // Number of LED rows (-, cathode)
#define NUM_BTN_COLS (4) // Number of switch columns (isolating diode anode)
#define NUM_BTN_ROWS (1) // Number of switch rows (isolating diode cathode)

// Debounce built-in to the code. This sets the number of button
// high or low senses that trigger a press or release
#define MAX_DEBOUNCE (3)

////////////////////
// Hardware Setup //
////////////////////
static const uint8_t btnRowPins[NUM_BTN_ROWS] = {2}; // Pins connected to switch rows (2)
static const uint8_t btnColPins[NUM_BTN_COLS] = {23, 22 ,21, 20}; // Pins connected to switch columns (1)
static const uint8_t ledRowPins[NUM_LED_ROWS] = {}; // Pins connected to LED rows (-)
static const uint8_t ledColPins[NUM_LED_COLS] = {};// Pins connected to LED cols (+)

//////////////////////
// Global Variables //
//////////////////////
static bool LED_buffer[NUM_LED_COLS][NUM_LED_ROWS]; // Keeps track of LED states
static int8_t debounce_count[NUM_BTN_COLS][NUM_BTN_ROWS]; // One debounce counter per switch

void setup()
{
  Serial.begin(9600);
  setupLEDPins();
  setupSwitchPins();

  // Initialize the debounce counter array
  for (uint8_t i = 0; i < NUM_BTN_COLS; i++)
  {
    for (uint8_t j = 0; j < NUM_BTN_ROWS; j++)
    {
      debounce_count[i][j] = 0;
    }
  }
  // Initialize the LED buffer
  for (uint8_t i = 0; i < NUM_LED_COLS; i++)
  {
    for (uint8_t j = 0; j < NUM_LED_ROWS; j++)
    {
      LED_buffer[i][j] = 0; // All LED's off
    }
  }

  // Joystick
  XInput.setJoystickRange(0, ADC_Max);  // Set joystick range to the ADC
	XInput.begin();
}

void loop() 
{
  scan();

  // Joystick

  int rightJoyX = analogRead(AXIS_X_PIN);
	int rightJoyY = analogRead(AXIS_Y_PIN);

	boolean invert = true;

	// You may have to flip these around
	XInput.setJoystickX(JOY_RIGHT, rightJoyX);
	XInput.setJoystickY(JOY_RIGHT, rightJoyY, invert);
  XInput.send();
}

static void scan()
{
  // Each run through the scan function operates on a single row
  // of the matrix, kept track of using the currentRow variable.
  static uint8_t currentRow = 0;
  uint8_t i, j; // for loop counters

  // Select current row, and write all components on that row LOW.
  // That'll set the LED anode's LOW, and write the switch "2" pins LOW.
  // If diodes were added, "2' should be connected to the diode cathode
  digitalWrite(btnRowPins[currentRow], LOW);
  digitalWrite(ledRowPins[currentRow], LOW);

  // Look at the LED_buffer variable along this row.
  // If any column is 1, turn the LED on.
  // Otherwise LED will be left off
  for (i = 0; i < NUM_LED_COLS; i++)
  {
    if (LED_buffer[currentRow][i])
    {
      digitalWrite(ledColPins[i], HIGH); // Turn LED on
    }
  }

  // Scan through switches on this row:
  for ( j = 0; j < NUM_BTN_COLS; j++)
  {
    // Read the button. If it's pressed, it should be LOW.
    if (digitalRead(btnColPins[j]) == LOW)
    {
      if ( debounce_count[currentRow][j] < MAX_DEBOUNCE)
      { // Increment a debounce counter
        debounce_count[currentRow][j]++;
        if ( debounce_count[currentRow][j] == MAX_DEBOUNCE )
        { // If debounce counter hits MAX_DEBOUNCE (default: 3)
          // Trigger key press -- Do anything here...
          Serial.print("Key pressed "); 
          Serial.println(j);
          XInput.press(j+1);
          LED_buffer[currentRow][j] = 1; // Set LED to turn on next time through
        }
      }
    }
    else // Otherwise, button is released
    {
      if ( debounce_count[currentRow][j] > 0)
      { 
        debounce_count[currentRow][j]--; // Decrement debounce counter
        if ( debounce_count[currentRow][j] == 0 )
        { // If debounce counter hits 0
          // Trigger key release -- Do anything here...
          Serial.print("Key released "); // Trigger key release
          XInput.release(j+1);
          Serial.println((currentRow * NUM_BTN_COLS) + j);
          LED_buffer[currentRow][j] = 0; // Set LED to turn off next time through
        }
      }
    }
  }

  // Once done scanning, de-select the switch and LED rows
  // by writing them HIGH.
  digitalWrite(btnRowPins[currentRow], HIGH);
  digitalWrite(ledRowPins[currentRow], HIGH);

  // Then turn off any LEDs that might have turned on:
  for (i = 0; i < NUM_LED_ROWS; i++)
  {
    digitalWrite(ledColPins[i], LOW);
  }

  // Increment currentRow, so next time we scan the next row
  currentRow++;
  if (currentRow >= NUM_LED_ROWS)
  {
    currentRow = 0;
  }
}

static void setupLEDPins()
{
  uint8_t i;

  // LED drive rows - written LOW when active, HIGH otherwise
  for (i = 0; i < NUM_LED_ROWS; i++)
  {
    pinMode(ledRowPins[i], OUTPUT);
    digitalWrite(ledRowPins[i], HIGH);
  }

  // LED select columns - Write HIGH to turn an LED on.
  for (i = 0; i < NUM_LED_COLS; i++)
  {
    pinMode(ledColPins[i], OUTPUT);
    digitalWrite(ledColPins[i], LOW);
  }
}

static void setupSwitchPins()
{
  uint8_t i;

  // Button drive rows - written LOW when active, HIGH otherwise
  for (i = 0; i < NUM_BTN_ROWS; i++)
  {
    pinMode(btnRowPins[i], OUTPUT);

    // with nothing selected by default
    digitalWrite(btnRowPins[i], HIGH);
  }

  // Buttn select columns. Pulled high through resistor. Will be LOW when active
  for (i = 0; i < NUM_BTN_COLS; i++)
  {
    pinMode(btnColPins[i], INPUT_PULLUP);
  }
}
