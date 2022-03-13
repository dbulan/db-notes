# VPS - NEW SUBDOMIAN

$ sudo mkdir -p /var/www/your_domain 					// Используя флаг -p для создания необходимых родительских директорий
$ sudo chown -R $USER:$USER /var/www/your_domain		// $USER - кем сейчас авторизированы, если нужен кто-то другой то вписываем john:john
$ sudo chmod -R 755 /var/www/your_domain

$ nano /var/www/your_domain/index.php

$ sudo nano /etc/nginx/sites-available/your_domain

# HTML
/**
server {
	listen 80;
	listen [::]:80;
	root /var/www/your_domain;
	index index.html index.htm index.nginx-debian.html;
	server_name your_domain www.your_domain;
	location / {
			try_files $uri $uri/ =404;
	}
}
*/

# PHP
/**
server {
	listen 80;
	listen [::]:80;
	root /var/www/your_domain;
	index index.html index.htm index.php;
	server_name your_domain www.your_domain;
	location / {
		try_files $uri $uri/ =404;
	}
	
	location ~ \.php$ {
        include snippets/fastcgi-php.conf;
        fastcgi_pass unix:/var/run/php/php7.4-fpm.sock;
	}
    location ~ /\.ht {
        deny all;
    }
}
*/

# CLOUDLFLARE HTTPS
/**
server {
    listen 80;
    listen [::]:80;
    server_name your_domain;
    return 302 https://$server_name$request_uri;
}

server {
	# SSL configuration
	listen 443 ssl http2;
	listen [::]:443 ssl http2;
	ssl_certificate         /etc/ssl/your_domain.cert.pem;
	ssl_certificate_key     /etc/ssl/your_domain.key.pem;
	ssl_client_certificate /etc/ssl/cloudflare.crt;
	ssl_verify_client on;

    server_name your_domain;

    root /var/www/your_domain;
    index index.php;

	location / {
		try_files $uri $uri/ =404;
	}

	location ~ \.php$ {
		include snippets/fastcgi-php.conf;
		fastcgi_pass unix:/var/run/php/php7.4-fpm.sock;
	}

	location ~ /\.ht {
		deny all;
	}
}
*/

$ sudo ln -s /etc/nginx/sites-available/your_domain /etc/nginx/sites-enabled/

$ sudo nginx -t
$ sudo systemctl restart nginx