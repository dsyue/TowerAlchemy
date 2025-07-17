extends Node2D

func _ready() -> void:
	# 创建5x5透明背景图像
	var img = Image.create(5, 5, false, Image.FORMAT_RGBA8)
	img.fill(Color.TRANSPARENT)  # 透明背景
	
	# 计算圆心和半径
	var center = Vector2(2.5, 2.5)  # 精确到浮点数确保中心对称
	var radius = 2.0
	
	# 绘制完美圆形 (使用抗锯齿)
	for y in range(5):
		for x in range(5):
			var pos = Vector2(x, y)
			var distance = pos.distance_to(center)
			
			# 抗锯齿处理：边缘像素部分透明
			if distance <= radius - 0.5:
				img.set_pixel(x, y, Color.WHITE)  # 完全在圆内
			elif distance < radius + 0.5:
				var alpha = 1.0 - (distance - (radius - 0.5))
				img.set_pixel(x, y, Color(1, 1, 1, alpha))  # 边缘半透明
	
	# 创建纹理
	var texture = ImageTexture.create_from_image(img)
	
	# 保存为资源文件
	var save_path = "res://Assets/Images/Bullets/white_bullet.tres"
	ResourceSaver.save(texture, save_path)
	print("子弹资源已保存至: ", save_path)
