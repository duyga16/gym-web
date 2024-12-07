CREATE DATABASE IF NOT EXISTS gym;
USE gym;

-- BẢNG MENU 
CREATE TABLE Menu (
    menuID INT AUTO_INCREMENT PRIMARY KEY,
    menuName NVARCHAR(255) NOT NULL,         
    link NVARCHAR(1000),                  
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP  
);

-- BẢNG TÀI KHOẢN NGƯỜI DÙNG
CREATE TABLE Accounts (
    accountID INT AUTO_INCREMENT PRIMARY KEY,  
    userName NVARCHAR(255) NOT NULL,       
    passwordHash NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) NOT NULL,
	Roles nvarchar(10),
    resetTokenExpires  DATETIME,
    resetToken NVARCHAR(255),
    acceptTerms TINYINT(1) NOT NULL DEFAULT 0,
    IsEmailConfirmed  TINYINT(1) NOT NULL DEFAULT 0,
    emailConfirmationToken NVARCHAR(255),
    phoneNumber NVARCHAR(15),
    roleAdmin TINYINT(1) NOT NULL DEFAULT 0,
    link NVARCHAR(1000),                  
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP  
);

-- BẢNG THÔNG TIN NGƯỜI DÙNG
CREATE TABLE Users (
    userID INT AUTO_INCREMENT PRIMARY KEY,  
    accountID INT,
    menuID INT,
    rankID INT,
    userName NVARCHAR(255) NOT NULL,       
	fullName NVARCHAR(255),       
    email NVARCHAR(255) NOT NULL,
    phoneNumber NVARCHAR(15),
    img NVARCHAR(255),
    price Decimal(15,2),
    rankPrice Decimal(15,2),
    countClass INT default 0,
    DateOfBirth DATE,
    gender NVARCHAR(10),
    addess NVARCHAR(1000),
    Roles NVARCHAR(10),
    FOREIGN KEY (accountID) REFERENCES Accounts(accountID) ON DELETE CASCADE,
	FOREIGN KEY (menuID) REFERENCES Menu(menuID) ON DELETE CASCADE,
    FOREIGN KEY (rankID) REFERENCES RankUser(rankID) ON DELETE CASCADE,
    link NVARCHAR(1000),                  
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- BẢNG LIÊN HỆ
CREATE TABLE Contact (
    contactID INT AUTO_INCREMENT PRIMARY KEY,  
    userID INT,
    phoneNumber NVARCHAR(15),
    userName NVARCHAR(255) NOT NULL,
    title NVARCHAR(255) NOT NULL,
	details TEXT NOT NULL,
	FOREIGN KEY (userID) REFERENCES Users(userID) ON DELETE CASCADE,
    link NVARCHAR(1000),                  
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP  
);

-- BẢNG KHO HÌNH ẢNH
CREATE TABLE Gallery (
    galleryID INT AUTO_INCREMENT PRIMARY KEY,  
    userID INT,
	linkImg NVARCHAR(255),
    isWide TINYINT(1) DEFAULT 0,
	FOREIGN KEY (userID) REFERENCES Users(userID) ON DELETE CASCADE,
    link NVARCHAR(1000),                  
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP  
);

-- BẢNG THÔNG TIN SỨC KHỎE
CREATE TABLE HealthInfo (
    healthID INT AUTO_INCREMENT PRIMARY KEY,  
    userID INT,
	Height DECIMAL(5, 2),
    Weight DECIMAL(5, 2),
    BMI DECIMAL(5, 2),
    age INT,
    gender NVARCHAR(10),
    BodyFatPercentage DECIMAL(5, 2),
	FOREIGN KEY (userID) REFERENCES Users(userID) ON DELETE CASCADE,
    link NVARCHAR(1000),                  
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP  
);

-- BẢNG PHÂN HẠNG, GÓI NGƯỜI DÙNG
CREATE TABLE RankUser(
    rankID INT AUTO_INCREMENT PRIMARY KEY,  
	membershipType VARCHAR(50),
    rankType VARCHAR(50),
    details NVARCHAR(255),
    startDate DATE,
    endDate DATE,
	price DECIMAL(15, 2),
    link NVARCHAR(1000),                  
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP  
);

-- BẢNG KHÓA TẬP
CREATE TABLE Course (
    courseID INT AUTO_INCREMENT PRIMARY KEY,  
    userID INT,
	nameCourse NVARCHAR(255) NOT NULL,
    tiltle NVARCHAR(255) NOT NULL,
    price DECIMAL(10, 2),
    details TEXT NOT NULL,
	FOREIGN KEY (userID) REFERENCES Users(userID) ON DELETE CASCADE,
    link NVARCHAR(1000),                  
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP  
);

-- BẢNG FEEDBACK
CREATE TABLE Feedback (
    feedbackID INT AUTO_INCREMENT PRIMARY KEY,  
    userID INT,
    ptID INT,
    newsID INT,
	details TEXT NULL,
    title NVARCHAR(50),
    rating INT CHECK (Rating >= 1 AND Rating <= 5),
	FOREIGN KEY (newsID) REFERENCES News(newsID) ON DELETE CASCADE,
    FOREIGN KEY (userID) REFERENCES Users(userID) ON DELETE CASCADE,
    FOREIGN KEY (ptID) REFERENCES PT(ptID) ON DELETE CASCADE,
    link NVARCHAR(1000),                  
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP  
);

-- BẢNG TIN TỨC
CREATE TABLE News (
    newsID INT AUTO_INCREMENT PRIMARY KEY,  
	newsName NVARCHAR(255) NOT NULL,
    script TEXT NOT NULL,
    img NVARCHAR(255),
    descriptions TEXT NOT NULL,
    detail LONGTEXT,
    title NVARCHAR(255),
    isLike INT DEFAULT 0,
    slogan NVARCHAR(255),
    link NVARCHAR(1000),     
    subMeta NVARCHAR(255),
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP  
);

-- BẢNG LỚP TẬP
CREATE TABLE Classes (
    classID INT AUTO_INCREMENT PRIMARY KEY,  
    nameClass NVARCHAR(255) NOT NULL,
    img NVARCHAR(255),
    price DECIMAL(15, 2),
    countUser INT default 0,
    descriptions TEXT NOT NULL,
    detail LONGTEXT,
    title NVARCHAR(255),
    link NVARCHAR(1000),                  
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP  
);

-- BẢNG LIÊN KẾT USER_CLASS
CREATE TABLE user_class(
	userID INT not null,              
    classID INT not null,             
    PRIMARY KEY (userID, classID),   
    FOREIGN KEY (userID) REFERENCES Users(userID) ON DELETE CASCADE,
    FOREIGN KEY (classID) REFERENCES Classes(classID) ON DELETE CASCADE,
    link NVARCHAR(1000),                  
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP  
);

-- BẢNG PT
CREATE TABLE PT(
	ptID INT AUTO_INCREMENT PRIMARY KEY,              
    classID INT, 
    namePT NVARCHAR(255) NOT NULL,
    email NVARCHAR(255),
    phoneNumber NVARCHAR(15),
    img NVARCHAR(255),
    height NVARCHAR(50),
    weight NVARCHAR(50),
	age INT,
	gender NVARCHAR(10),
	skills NVARCHAR(100),
    specialty NVARCHAR(255),
    experienceYears INT,
    rating INT CHECK (Rating >= 1 AND Rating <= 5) DEFAULT 5,
    descriptions TEXT,
    slogan NVARCHAR(255),
    FOREIGN KEY (classID) REFERENCES Classes(classID) ON DELETE CASCADE,
    link NVARCHAR(1000),                  
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP  
);

-- BẢNG LỊCH TẬP
CREATE TABLE Schdule(
	scheduleID INT AUTO_INCREMENT PRIMARY KEY,      
    classID INT not null, 
    ptID INT,
	dayOfWeeks ENUM('Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday') NOT NULL,
	timeSlot ENUM('06:00 - 08:00', '10:00 - 12:00', '1:00 - 3:00', '5:00 - 7:00','7:00 - 9:00') NOT NULL,
	nameClass NVARCHAR(255),
    namePT NVARCHAR(255),
    dateCreate DATE,
    FOREIGN KEY (classID) REFERENCES Classes(classID) ON DELETE CASCADE,
	FOREIGN KEY (ptID) REFERENCES PT(ptID) ON DELETE CASCADE,
    link NVARCHAR(1000),                  
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP  
);

-- BẢNG User vs Schedule
CREATE TABLE UserSchedule (
    UserScheduleID INT AUTO_INCREMENT PRIMARY KEY,
    userID INT,                                   
    scheduleID INT,                              
    FOREIGN KEY (userID) REFERENCES Users(userID) ON DELETE CASCADE, 
    FOREIGN KEY (scheduleID) REFERENCES Schdule(scheduleID) ON DELETE CASCADE,
    link NVARCHAR(1000),                  
    meta NVARCHAR(255),
    hide TINYINT(1) DEFAULT 1,         
    `order` INT NOT NULL,               
    datebegin DATETIME DEFAULT CURRENT_TIMESTAMP  
);


