#include <SFML/Graphics.hpp>

int main() {
    // Get the resolution of the screen
    sf::VideoMode fullscreen = sf::VideoMode::getDesktopMode();

    // Create a full-screen window
    sf::RenderWindow window(fullscreen, "SFML window", sf::Style::Fullscreen);

    while (window.isOpen()) {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed)
                window.close();
        }

        window.clear();
        // Draw your graphics here
        window.display();
    }

    return 0;
}
