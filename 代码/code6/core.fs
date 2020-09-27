#version 330 core
in vec2 textcoords;
uniform sampler2D Texture;
out vec4 color;
void main()
{
color = vec4(texture(Texture,textcoords).rgb,0.2f);//vec4(1.0f,0.0f,0.0f, 1.0f);
}
