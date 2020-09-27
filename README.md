# Computer-Graphics
## 目录
+ 第一节课 [创建窗口](https://github.com/chenyihangis/Computer-Graphics/blob/master/README.md#%E7%AC%AC%E4%B8%80%E8%8A%82%E8%AF%BE-%E5%88%9B%E5%BB%BA%E7%AA%97%E5%8F%A3)
+ 第二节课 [画三角形](https://github.com/chenyihangis/Computer-Graphics/blob/master/README.md#%E7%AC%AC%E4%BA%8C%E8%8A%82%E8%AF%BE-%E7%94%BB%E4%B8%89%E8%A7%92%E5%BD%A2)
+ 第三节课 [画三角形（二）](https://github.com/chenyihangis/Computer-Graphics/blob/master/README.md#%E7%AC%AC%E4%B8%89%E8%8A%82%E8%AF%BE-%E7%94%BB%E4%B8%89%E8%A7%92%E5%BD%A2%E4%BA%8C)
+ 第四节课 [画矩形和彩色三角形](https://github.com/chenyihangis/Computer-Graphics/blob/master/README.md#%E7%AC%AC%E5%9B%9B%E8%8A%82%E8%AF%BE-%E7%94%BB%E7%9F%A9%E5%BD%A2%E5%92%8C%E5%BD%A9%E8%89%B2%E4%B8%89%E8%A7%92%E5%BD%A2)
+ 第五节课 [添加纹理（一）](https://github.com/chenyihangis/Computer-Graphics/blob/master/README.md#%E7%AC%AC%E4%BA%94%E8%8A%82%E8%AF%BE-%E6%B7%BB%E5%8A%A0%E7%BA%B9%E7%90%86%E4%B8%80)
+ 第六节课 [添加纹理（二）](https://github.com/chenyihangis/Computer-Graphics/blob/master/README.md#%E7%AC%AC%E5%85%AD%E8%8A%82%E8%AF%BE-%E6%B7%BB%E5%8A%A0%E7%BA%B9%E7%90%86%E4%BA%8C)
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
VAO将数据和显存中的物理地址相对应，数据放在VBO里，VAO与VBO成对出现;一组数据若有不同的解读方式，可能有多个VAO，一个VAO一定要对应一个VBO；
VAO用来解释VBO中的数据如何处理
```cpp
GLuint VAO, VBO;
glGenVertexArrays(1, &VAO);
glGenBuffers(1, &VBO);
//相绑定
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
将编译、链接shader的部分放在shader.h中，并将着色器放入文件res/shader中，在主函数中的while循环里使用Use()函数。
## 第四节课 画矩形和彩色三角形
### 画矩形方法一：画两个三角形
```cpp
GLfloat vertices[] =
{
	//first triangle
	0.5f,0.5f,0.0f, //top right
	0.5f,-0.5f,0.0f, //bottom right
	-0.5f,0.5f,0.0f, //top left

	//second triangle
	0.5f,-0.5f,0.0f, // bottom right
	-0.5f,-0.5f,0.0f, //bottom left
	-0.5f,0.5f,0.0f //top left
};
//画图的循环里改为6个点
glDrawArrays(GL_TRIANGLES, 0, 6)
```
### 画矩形方法二：索引顶点坐标
+ 优点：节约了存放顶点坐标的空间
+ 注意：这里的顶点颜色信息要一致，否则就是不同的顶点
```cpp
GLfloat vertices[] =
{
	0.5f,0.5f,0.0f, //top right
	0.5f,-0.5f,0.0f, //bottom right
	-0.5f,-0.5f,0.0f, //bottom left
	-0.5f,0.5f,0.0f //top left
};
```
索引顶点的序号
```cpp
unsigned int indices[] =
{
	0,1,3, //first triangle
	1,2,3 //second triangle
};
```
需要用EBO建立连接信息
```cpp
GLuint EBO;
glGenBuffers(1, &EBO);
glBindBuffer(GL_ELEMENT_ARRAY_BUFFER,EBO);
glBufferData(GL_ELEMENT_ARRAY_BUFFER,sizeof(indices), indices, GL_STATIC_DRAW);
glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
```
在循环中绑定EBO,并改变画图方式
```cpp
glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);
```
最后删除`glDeleteBuffers(1, &EBO);`
### 画彩色三角形
将颜色信息写入顶点中(删除原先的EBO的操作)
```cpp
GLfloat vertices[] =
{
	//position			//	color
	-0.5f,-0.5f,0.0f, 1.0f,0.0f,0.0f,
	0.5f,-0.5f,0.0f,  0.0f,1.0f,0.0f,
	0.0f,0.5f,0.0f,   0.0f,0.0f,1.0f    
};
```
顶点着色器：顶点着色器没有输入，需要从显存里获取数据，得通过VAO获得。如location=0意味着从VAO中第一条指引中查找解读显存数据的方式。out输出。
```
#version 330 core
layout(location = 0) in vec3 position;
layout (location = 1) in vec3 color;
out vec3 outColor;
void main()
{
gl_Position = vec4(position.x, position.y, position.z, 1.0f);
outColor = color;
}
```
边缘着色器：in传进。边缘着色器可以没有输出，这样就不画颜色，有输出一定就是唯一的输出颜色
```
#version 330 core
in vec3 outColor;
out vec4 color;
void main()
{
color = vec4(outColor, 1.0f);
}
```
改变参数,画图用原先的
```cpp
glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (GLvoid*)0);//隔6个取
glEnableVertexAttribArray(0);
glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (GLvoid*)(3*sizeof(GLfloat)));//从起始位置偏移3个浮点型数据位置开始
glEnableVertexAttribArray(1);
//画图
glDrawArrays(GL_TRIANGLES, 0, 6);
```
## 第五节课 添加纹理（一）
在物体表面添加图片信息
### 纹理相关介绍
+ 纹理坐标：左下角(0,0)；右上角(1,1)；UV坐标轴
+ 超过纹理坐标范围，处理环绕方式有   
1. 重复：GL_REPEAT，即超过范围重复图案，边缘的衔接不是特别好
2. 镜像：GL_MIRRORED_REPEAT，即超过范围镜像表现图案，边缘衔接很好
3. 拉伸：GL_CLAMP_TO_EDGE，即超过范围会重复纹理坐标的边缘
4. 自定边缘颜色：GL_CLAMP_TO_BORDER，超过范围为自定的边缘颜色
---
+ 纹理过滤：
1. GL_NEAREST:距离最近过滤,选择离中心点最接近纹理坐标的那个像素。
2. GL_LINEAR:线性过滤，基于纹理坐标附近的纹理像素，进行线性插值，近似出附近纹理像素之间的颜色。一个纹理像素的中心距离纹理坐标越近，这个纹理像素的颜色对最终样本颜色的影响越大
3. Mipmap：是一系列图案相同而大小不同的纹理图像。产生一个纹理金字塔，第0层是图像的原图，经过高斯滤波产生下一层，后一层的纹理图像是前一个的一半。
---
+ 纹理的分辨率与映射物体的大小的相关处理：
1. 在一个很大的物体上应用一张低分辨率的纹理(用GL_NEAREST和GL_LINEAR)：
   1. GL_LINEAR图案边缘平滑
   2. GL_NEAREST图案边缘产生了锯齿，整体相对GL_LINEAR较清晰
2. 纹理有较高的分辨率和映射物体较小：在Mipmap中选择最适合物体的大小的那个
---
总结：
+ 当纹理分辨率低而物体大时，GL_NEAREST和GL_LINEAR作为进行放大和缩小操作时纹理过滤的选择
+ 当纹理分辨率高而物体小时，用Mipmap
### 着色器
顶点着色器：读进二维的纹理坐标，并输出纹理坐标
```cpp
layout (location = 1) in vec2 textCoords;
out vec2 textcoords;
textcoords = textCoords;//main中
```
边缘着色器：输入纹理坐标
```in vec2 textcoords;```
### 添加纹理
头文件
```cpp
#include "SOIL2/SOIL2.h"
#include "SOIL2/stb_image.h"
```
创建纹理对象并绑定
```cpp
GLuint texture;
glGenTextures(1, &texture);
glBindTexture(GL_TEXTURE_2D, texture);
```
导入图片
```cpp
unsigned char* image =SOIL_load_image("res/images/image1.jpg", &width, &height, 0, SOIL_LOAD_RGBA);
```
设置
```cpp
glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);//对U轴设置(S对应U轴)
glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);//对V轴设置(T对应V轴)
//放大和缩小如何与纹理坐标相适应
glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
```
## 第六节课 添加纹理（二）
### 接上
将图片从内存中传入显存里,glTexImage2D的各个参数：
+ 第二个参数：第0层图片；第三个参数：RGBA传入三个颜色加透明度信息；第四个第五个参数：图片的大小即宽和高
+ 第六个参数：边界处理；第七个参数：从图片里获得RGBA信息；第八个参数：图片数据类型；第九个参数：图片变量
```cpp
glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, image);
SOIL_free_image_data(image);//释放图片变量
glGenerateMipmap(GL_TEXTURE_2D);
glBindTexture(GL_TEXTURE_2D, 0);
```
传入着色器的两种方式：
+ 静态：通过VAO（layout location）
+ 动态：即允许在运行过程中传入，使用标志位uniform（大小不超过1024K）
### 边缘着色器
加入,sample2D的类型为整型，是二维纹理，做采样
```cpp
uniform sampler2D Texture;
```
主函数里改为
```cpp
color=texture(Texture,textcoords);
```
若想设置透明度，下面的第二个参数表示透明度，以前均设为1.0f
```cpp
color = vec4(texture(Texture,textcoords).rgb,0.2f)
```
---
在while循环里，开启一个标志位，不需要创建对象，与VAO不同（需要创建对象）；因为图片信息简单，只包含数据类型和图片的宽高
```cpp
glActiveTexture(GL_TEXTURE0);//GL_TEXTURE0已预设好了，相当于直接索引
```
将texture信息填入TEXTURE0
```cpp
glBindTexture(GL_TEXTURE_2D, texture);
```
glUniform* 函数,数字表示传进几个，第二个表示传进来的数据类型，i是整型，f是浮点型
```cpp
glUniform1i(glGetUniformLocation(shader.Program, "Texture"), 0);//第二个参数的“0”表示的就是TEXTURE0
```
最后删除```glDeleteTextures(1, &texture);```
### 图像颠倒
完成以上操作，最后的运行结果图像是颠倒的。这是因为OpenGL的纹理坐标原点在图片最下面，而图片信息中的原点一般都在最上方。修改顶点着色器里的纹理坐标即可
```cpp
textcoords = vec2(textCoords.x,1-textCoords.y);
```
