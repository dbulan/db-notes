# VPS - FULL

-----------------------------------------------------------------------------------------------------------------

# Процедура установки и настройки Laravel с Nginx на сервере Ubuntu 20.04
https://www.digitalocean.com/community/tutorials/how-to-install-and-configure-laravel-with-nginx-on-ubuntu-20-04-ru

-----------------------------------------------------------------------------------------------------------------

# Шаг 1 — Установка необходимых модулей PHP

$ sudo apt install php-mbstring php-xml php-bcmath

$ cd /var/www/domain.com

// New Project
$ rm -r * // (!) Удаляя саму папку есть вероятность, что нужно перенастраивать chown -R $USER, sites-available etc, поэтому чистим все внутри
$ sudo composer create-project --prefer-dist laravel/laravel:7.* .
$ sudo composer install
//

// Git

$ ls -l
$ rm -r * // zachiwaem, no ostajutsa hidden files (esli oni byli)

$ ls -a // hidden
$ rm -rf ..?* .[!.]* * // zachiwaem hidden files

$ git clone https://****.git .
$ composer install
$ npm install


$ php artisan

$ sudo chown -R www-data.www-data /var/www/domain.com/storage
$ sudo chown -R www-data.www-data /var/www/domain.com/bootstrap/cache
$ sudo chmod -R 775 storage // mozet i nenado prst kogda ne smog zapisat v log oshibku, zabil verhnie komandi sdelatj
$ sudo php artisan storage:link

---  
??? $ sudo php artisan storage:link
----

// --------------- Node and Npm ---------------
$ sudo apt update
$ sudo apt install nodejs
$ node -v // v10, no nuzno bolse v14

// If you try installing the latest version of node using the apt-package manager, you'll end up with v10.19.0. This is the latest version in the ubuntu app store, but it's not the latest released version of NodeJS.
https://www.freecodecamp.org/news/how-to-install-node-js-on-ubuntu-and-update-npm-to-the-latest-version/

// Using NVM - my preferred method
# Install NVM
$ sudo curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.35.3/install.sh | bash
$ nvm --version
// (!!!) Restart your terminal for your changes to take effect.
$ nvm install 14.17.4 // po default srazu perekljuchitsa na etu
$ nvm ls // spisok nodes
// $ nvm use v14.17.4

$ sudo apt install npm
$ npm -v

$ cd to project
$ npm install

// Vue Error
webpack.mix.js // added .vue()
mix.js('resources/js/app.js', 'public/js').vue().sass('resources/sass/app.scss', 'public/css');

// --------------- Node and Npm ---------------

https://www.digitalocean.com/community/tutorials/how-to-install-node-js-on-ubuntu-20-04-ru
$ sudo apt install cron
$ sudo systemctl enable cron

$ crontab -e
1. /bin/nano        <---- easiest

* * * * * php /var/www/domain.ru/artisan schedule:run >> /dev/null 2>&1
// sam zarabotal bez cron reload

$ git config user.name ""
$ git config user.email ""

$ git pull

# Laravel

/**
server {
	listen 80;
	server_name dev.dbulan.com www.dev.dbulan.com;
	root /var/www/dev.dbulan.com/public;

	add_header X-Frame-Options "SAMEORIGIN";
	add_header X-XSS-Protection "1; mode=block";
	add_header X-Content-Type-Options "nosniff";

	index index.html index.htm index.php;

	charset utf-8;

	location / {
		try_files $uri $uri/ /index.php?$query_string;
	}

	location = /favicon.ico { access_log off; log_not_found off; }
	location = /robots.txt  { access_log off; log_not_found off; }

	error_page 404 /index.php;

	location ~ \.php$ {
		fastcgi_pass unix:/var/run/php/php7.4-fpm.sock;
		fastcgi_index index.php;
		fastcgi_param SCRIPT_FILENAME $realpath_root$fastcgi_script_name;
		include fastcgi_params;
	}

	location ~ /\.(?!well-known).* {
		deny all;
	}
}
*/
	
# Laravel + Cloudflare
/**
server {
    listen 80;
    listen [::]:80;
    server_name dev.vsmuta.ru;
    return 302 https://$server_name$request_uri;
}

server {
	# SSL configuration
	
	listen 443 ssl http2;
	listen [::]:443 ssl http2;
	ssl_certificate         /etc/ssl/vsmutaru.cert.pem;
	ssl_certificate_key     /etc/ssl/vsmutaru.key.pem;
	ssl_client_certificate /etc/ssl/cloudflare.crt;
	ssl_verify_client on;

    server_name dev.vsmuta.ru;

    root /var/www/dev.vsmuta.ru/public;
    index index.php;
	
	add_header X-Frame-Options "SAMEORIGIN";
	add_header X-XSS-Protection "1; mode=block";
	add_header X-Content-Type-Options "nosniff";

	charset utf-8;

	location / {
		try_files $uri $uri/ /index.php?$query_string;
	}
	
	location = /favicon.ico { access_log off; log_not_found off; }
	location = /robots.txt  { access_log off; log_not_found off; }

	error_page 404 /index.php;

	location ~ \.php$ {
		fastcgi_pass unix:/var/run/php/php7.4-fpm.sock;
		fastcgi_index index.php;
		fastcgi_param SCRIPT_FILENAME $realpath_root$fastcgi_script_name;
		include fastcgi_params;
	}

	location ~ /\.(?!well-known).* {
		deny all;
	}
}
*/


$ sudo nginx -t
$ sudo systemctl reload nginx

# Prodolzaem nastrojku ENV
.env propisivaem connect db

$ sudo composer require laravel/ui:^2.4
$ php artisan ui vue --auth
$ php artisan migrate:refresh --seed



-----------------------------------------------------------------------------------------------------------------

### How To Host a Website Using Cloudflare and Nginx on Ubuntu 20.04
https://www.digitalocean.com/community/tutorials/how-to-host-a-website-using-cloudflare-and-nginx-on-ubuntu-20-04

-----------------------------------------------------------------------------------------------------------------

# Step 1 — Generating an Origin CA TLS Certificate

Dashboard -> SSL/TLS -> Origin Server -> [Create Certificate]:button

/**
	[v] Generate private key and CSR with Cloudflare
		Private key type: RSA (2048) v
		
	[ ]	Use my private key and CSR

	*.domain.com domain.com

	15 years
*/

# Next

Origin Certificate -> /etc/ssl/vsmutaru.cert.pem
Private Key		   -> /etc/ssl/vsmutaru.key.pem

$ sudo nano /etc/ssl/vsmutaru.cert.pem
$ sudo nano /etc/ssl/vsmutaru.key.pem

# Step 2 — Installing the Origin CA Certificate in Nginx

$ sudo ufw allow 'Nginx Full'
$ sudo ufw reload
$ sudo ufw status

/**
	Output
	Status: active

	To                         Action      From
	--                         ------      ----
	OpenSSH                    ALLOW       Anywhere
	Nginx Full                 ALLOW       Anywhere
	OpenSSH (v6)               ALLOW       Anywhere (v6)
	Nginx Full (v6)            ALLOW       Anywhere (v6)
*/

# /etc/nginx/sites-available/domain.com

/**
server {
    listen 80;
    listen [::]:80;
    server_name your_domain www.your_domain;
    return 302 https://$server_name$request_uri;
}

server {

    # SSL configuration

    listen 443 ssl http2;
    listen [::]:443 ssl http2;
    ssl_certificate         /etc/ssl/cert.pem;
    ssl_certificate_key     /etc/ssl/key.pem;

    server_name your_domain www.your_domain;

    root /var/www/your_domain/html;
    index index.html index.htm index.nginx-debian.html;


    location / {
            try_files $uri $uri/ =404;
    }
}
*/

$ sudo nginx -t
$ sudo systemctl restart nginx

# (!) Budet oshibka poka ne sdelaju fix kotorij nizhe!

Dashboard -> SSL/TLS -> Overview -> [change SSL/TLS encryption mode to Full (strict)]

# FIX: ERR_TOO_MANY_REDIRECTS (Esli ne vkljuchil FULL STRICT v Cloudflare) (ne proverjal)
// Since you are using cloudflare flexible SSL your nginx config file wll look like this:

/**
server {
	listen 80 default_server;
	listen [::]:80 default_server;
	server_name mydomain.com www.mydomain.com;

	if ($http_x_forwarded_proto = "http") {
		return 301 https://$server_name$request_uri;
	}

	root /var/www/html;

	index index.php index.html index.htm index.nginx-debian.html;

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

// Na etom etape uze vse OK rabotaet, dalshe hz zachem

# Step 3 — Setting Up Authenticated Origin Pulls

// OPEN: https://developers.cloudflare.com/ssl/origin-configuration/authenticated-origin-pull/set-up/

Zone-Level -> Cloudflare certificate -> [download the .PEM file] -> /etc/ssl/cloudflare.crt;

$ /etc/nginx/site-available/domain.com

/**
server {
    # SSL configuration

    listen 443 ssl http2;
    listen [::]:443 ssl http2;
    ssl_certificate         /etc/ssl/cert.pem;
    ssl_certificate_key     /etc/ssl/key.pem;
	ssl_client_certificate /etc/ssl/cloudflare.crt;
	ssl_verify_client on;

    server_name your_domain www.your_domain;

    root /var/www/your_domain/html;
    index index.html index.htm index.nginx-debian.html;


    location / {
            try_files $uri $uri/ =404;
    }
}
*/

$ sudo nginx -t
$ sudo systemctl restart nginx

# Finally, to enable Authenticated Pulls
Dashboard -> SSL/TLS -> Origin Server -> Authenticated Origin Pulls

# No teper IP ne nastojawie, esli moj ip 8.8.8.8 to vizvav REMOTE_ADDR ja uvizu 9.9.9.9
https://support.cloudflare.com/hc/en-us/articles/200170786

# PHP Solution

// php reshenie problemi, no v error logs budet vseravno  ne pravelno
<?php if (isset($_SERVER['HTTP_CF_CONNECTING_IP'])) $_SERVER['REMOTE_ADDR'] = $_SERVER['HTTP_CF_CONNECTING_IP'];?>

# Nginx Solution

$ sudo /etc/nginx/nginx.conf -> posle gzip propisivaem

/**
	set_real_ip_from 103.21.244.0/22;
	set_real_ip_from 103.22.200.0/22;
	set_real_ip_from 103.31.4.0/22;
	set_real_ip_from 104.16.0.0/13;
	set_real_ip_from 104.24.0.0/14;
	set_real_ip_from 108.162.192.0/18;
	set_real_ip_from 131.0.72.0/22;
	set_real_ip_from 141.101.64.0/18;
	set_real_ip_from 162.158.0.0/15;
	set_real_ip_from 172.64.0.0/13;
	set_real_ip_from 173.245.48.0/20;
	set_real_ip_from 188.114.96.0/20;
	set_real_ip_from 190.93.240.0/20;
	set_real_ip_from 197.234.240.0/22;
	set_real_ip_from 198.41.128.0/17;
	set_real_ip_from 2400:cb00::/32;
	set_real_ip_from 2606:4700::/32;
	set_real_ip_from 2803:f800::/32;
	set_real_ip_from 2405:b500::/32;
	set_real_ip_from 2405:8100::/32;
	set_real_ip_from 2c0f:f248::/32;
	set_real_ip_from 2a06:98c0::/29;

	real_ip_header CF-Connecting-IP;
	#real_ip_header X-Forwarded-For; // ljuboj iz etih dvux
*/

$ sudo nginx -t 
$ sudo systemctl reload nginx

// Profit

-----------------------------------------------------------------------------------------------------------------