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
```
获取虚拟缓存对应的真缓存中的大小，包含32位颜色缓存，24位深度缓存，8位模板缓存
```cpp
glfwGetFramebufferSize(window, &screenWidth, &screenHeight);
```
指定当前窗口
```cpp
glfwMakeContextCurrent(window);
```
画图
```cpp
while (!glfwWindowShouldClose(window)) {
	glViewport(0, 0, screenWidth, screenHeight);
	glfwPollEvents();//执行事件
	glClearColor(0.2f, 0.3f, 0.3f, 1.0f);//最后一个参数是透明度，范围0~1
	glClear(GL_COLOR_BUFFER_BIT);//清除颜色缓存

	glfwSwapBuffers(window);//开启双缓存，一个在画一个在传送到屏幕
}
glfwTerminate();
```
## 第二节课 画三角形
### 顶点着色器
版本
```cpp
const GLchar* vertexShaderSource = "#version 330 core\n" 
```
position变量名；vec3(三维向量)类型，向量的每个分量都是浮点型；in表示这个变量要从外界输入；gl_Position预保留的变量，包含顶点信息，不需要定义；齐次坐标器（增加一维）
```cpp
"layout(location = 0) in vec3 position;\n"  
"void main()\n"
"{\n"
"gl_Position = vec4(position.x, position.y, position.z, 1.0f);\n"
"}";
```
### 边缘着色器
```cpp
const GLchar* fragmentShaderSource = "#version 330 core\n"
"out vec4 color;\n"
"void main()\n"
"{\n"
"color = vec4(1.0f, 0.5f, 0.2f, 1.0f);\n"
"}";
```
### import and compile the shader
```cpp
GLuint vertexShader = glCreateShader(GL_VERTEX_SHADER);
glShaderSource(vertexShader, 1, &vertexShaderSource, NULL);
glCompileShader(vertexShader);
//check 
GLint success;
GLchar infoLog[512];
glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
if (!success) {
	glGetShaderInfoLog(vertexShader, 512, NULL, infoLog);
	std::cout << "ERROR::SHADER::VERTEX::COMPILATION_FAILED\n"
		<< infoLog << std::endl;
}

GLuint fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
glShaderSource(fragmentShader, 1, &fragmentShaderSource, NULL);
glCompileShader(fragmentShader);

glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);
if (!success) {
	glGetShaderInfoLog(fragmentShader, 512, NULL, infoLog);
	std::cout << "ERROR::SHADER::FRAGMENT::COMPILATION_FAILED\n"
		<< infoLog << std::endl;
}
```
### creat the program and link the program
```cpp
GLuint shaderProgram = glCreateProgram();
glAttachShader(shaderProgram, vertexShader);
glAttachShader(shaderProgram, fragmentShader);
glLinkProgram(shaderProgram);
	
glGetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
if (!success) {
	glGetProgramInfoLog(shaderProgram, 512, NULL, infoLog);
	std::cout << "ERROR::SHADER::PROGRAM::LINK_FAILED\n"
		<< infoLog << std::endl;
}
glDeleteShader(vertexShader);
glDeleteShader(fragmentShader);
```
### Draw Triangle
```cpp
GLfloat vertices[] =
{
	-0.5f,-0.5f,0.0f,0.5f,-0.5f,0.0f,0.0f,0.5f,0.0f
};
```
VAO将数据和显存中的物理地址相对应，数据放在VBO里，VAO与VBO成对出现;一组数据若有不同的解读方式，可能有多个VAO，一个VAO一定要对应一个VBO
```cpp
GLuint VAO, VBO;
glGenVertexArrays(1, &VAO);
glGenBuffers(1, &VBO);
glBindVertexArray(VAO);
glBindBuffer(GL_ARRAY_BUFFER, VBO);
```
把顶点数据传输到显存的物理地址上
```cpp
glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
```
参数“0”对应VAO的第一行，参数“3”和“ GL_FLOAT”输入三个浮点型，“ GL_FALSE”表示不需要进行标准化；“3 * sizeof(GLfloat)”表示步长，“(GLvoid*)0”第一个起始点在哪找
```cpp
glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(GLfloat), (GLvoid*)0);
```
允许使用
```cpp
glEnableVertexAttribArray(0);
```
解绑，与绑定顺序相反
```cpp
glBindBuffer(GL_ARRAY_BUFFER, 0);
glBindVertexArray(0);
```
在while中加入
```cpp
glUseProgram(shaderProgram);
glBindVertexArray(VAO);
glDrawArrays(GL_TRIANGLES, 0, 3);
glBindVertexArray(0);
```
最后
```cpp
glDeleteVertexArrays(1, &VAO);
glDeleteBuffers(1, &VBO);
```
## 第三节课 画三角形（二）
将编译、链接shader的部分放在shader.h中，并将着色器放入文件res/shader中
