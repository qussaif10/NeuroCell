cmake_minimum_required(VERSION 3.27)
project(NeuroCell)
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

find_package(OpenGL REQUIRED)
find_package(glfw3 3.3 REQUIRED)
find_package(SFML COMPONENTS system window graphics CONFIG REQUIRED)

add_executable(NeuroCell main.cpp)

target_link_libraries(NeuroCell sfml-system sfml-network sfml-graphics sfml-window)
target_link_libraries(NeuroCell OpenGL::GL)
target_link_libraries(NeuroCell glfw)

