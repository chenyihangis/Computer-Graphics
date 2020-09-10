# Computer-Graphics
## 第一节课 创建窗口
初始化、配置版本
```cpp
glfwInit();
glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
```
窗口设置
```cpp
glfwWindowHint(GLFW_OPENGL_PROFILE,GLFW_OPENGL_CORE_PROFILE);
glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);
glfwWindowHint(GLFW_RESIZABLE, GL_FALSE);
```
创建窗口
```cpp
GLFWwindow* window = glfwCreateWindow(WIDTH, HEIGHT, "Learn OpenGL XXX", nullptr, nullptr);
glfwGetFramebufferSize(window, &screenWidth, &screenHeight);
glfwMakeContextCurrent(window);
```
画图
```cpp
while (!glfwWindowShouldClose(window)) {
	glViewport(0, 0, screenWidth, screenHeight);
	glfwPollEvents();
	glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
	glClear(GL_COLOR_BUFFER_BIT);

	glfwSwapBuffers(window);
}
glfwTerminate();
```
