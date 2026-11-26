-- Synthetic schema and data for the usage insights tests. It has the columns of the real gs_data_analysis database (discovered on 2026-11-12).
-- It contains no real data. Every figure the tests expect can be checked by hand against this file.
-- Dates are UTC. The tests read days in Puerto Rico time (UTC-4), so 03:59:59 UTC is the last second of the previous local day
-- and 04:00:00 UTC is the first second of the next local day.
SET time_zone = '+00:00';
DROP TABLE IF EXISTS vw_teacher_activities;
DROP TABLE IF EXISTS subscription_profile_history;
DROP TABLE IF EXISTS result_generic;
DROP TABLE IF EXISTS connected_date;
DROP TABLE IF EXISTS subject;
DROP TABLE IF EXISTS user;
DROP TABLE IF EXISTS school;
DROP TABLE IF EXISTS role;

CREATE TABLE role (role_id INT PRIMARY KEY, role VARCHAR(45));
CREATE TABLE school (school_id INT PRIMARY KEY, school_name VARCHAR(250), school_type VARCHAR(30), city VARCHAR(100), ore VARCHAR(100), ore_id INT,
  school_identifier VARCHAR(100), state_iso_code VARCHAR(45), country VARCHAR(150), country_iso VARCHAR(45));
CREATE TABLE user (user_id INT PRIMARY KEY, username VARCHAR(100), email VARCHAR(200), first_name VARCHAR(100), last_name VARCHAR(155),
  role_id INT NOT NULL, role VARCHAR(45) NOT NULL, school_id INT NULL, sie VARCHAR(50), ore_id INT, invitation_code VARCHAR(100));
CREATE TABLE connected_date (id INT AUTO_INCREMENT PRIMARY KEY, user_id INT, connected_date TIMESTAMP NULL, subscription_status VARCHAR(25));
CREATE TABLE subject (id INT PRIMARY KEY, subject_code VARCHAR(15), subject_name VARCHAR(100));
CREATE TABLE result_generic (id INT AUTO_INCREMENT PRIMARY KEY, user_id INT, subject_code VARCHAR(15), quiz_assigned_to_plan_date TIMESTAMP NULL,
  quiz_taken_date TIMESTAMP NULL, quiz_title VARCHAR(1000), total_percent_full_quiz DECIMAL(6,2) NULL);
-- The real table has NO id column; it only has an invisible key (my_row_id) that queries must not use.
CREATE TABLE subscription_profile_history (my_row_id BIGINT NOT NULL AUTO_INCREMENT INVISIBLE PRIMARY KEY, subscription_id INT NOT NULL, status VARCHAR(20) NOT NULL,
  activated_date DATETIME NULL, cancellation_date DATETIME NULL, user_attached_date DATETIME NULL, user_id INT NOT NULL, plan_name VARCHAR(100) NULL, plan_id INT NULL);
-- In production this is a VIEW that crosses other databases; here it is a table with the same columns.
CREATE TABLE vw_teacher_activities (teacher_name VARCHAR(200), invitation_code VARCHAR(100), sie VARCHAR(100), ore VARCHAR(100), school VARCHAR(250), school_number VARCHAR(50),
  from_period DATETIME, to_period DATETIME, has_connections_current_period VARCHAR(2), has_created_instructional_plans_current_period VARCHAR(2),
  plans_have_data_current_period VARCHAR(2), has_created_groups_current_year VARCHAR(2), has_created_assignments_overall VARCHAR(2));

INSERT INTO role VALUES (1,'client'),(2,'admin'),(3,'student'),(5,'teacher'),(6,'director'),(8,'admin-ore');

-- Schools: 1-3 are real; 4 has no users; 5 has users and zero connections; 6 belongs to the excluded ORE; 7 is excluded by name.
INSERT INTO school (school_id, school_name, school_type, city, ore, ore_id, country, country_iso) VALUES
 (1,'North High','Public','Test City','ORE ONE',1,'Puerto Rico','PR'),
 (2,'South High','Public','Test City','ORE ONE',1,'Puerto Rico','PR'),
 (3,'East Academy','Public','Test City','ORE TWO',2,'Puerto Rico','PR'),
 (4,'Empty School','Public','Test City','ORE ONE',1,'Puerto Rico','PR'),
 (5,'Silent School','Public','Test City','ORE ONE',1,'Puerto Rico','PR'),
 (6,'Demo School','Public','Test City','DEMOSTRACION',9,'Puerto Rico','PR'),
 (7,'Facilitators School','Public','Test City','ORE ONE',1,'Puerto Rico','PR');

-- Users. 12 has an internal email (excluded by domain). 80 has no school. 90 has the client role. 61 and 60 belong to the excluded ORE.
INSERT INTO user (user_id, username, email, first_name, last_name, role_id, role, school_id) VALUES
 (1,'user1','user1@example.invalid','Ada','Admin',2,'admin',NULL),
 (10,'user10','user10@example.invalid','Dina','Director',6,'director',1),
 (11,'user11','user11@example.invalid','Tom','Teacher',5,'teacher',1),
 (12,'user12','tina@internal.example','Tina','Teacher',5,'teacher',1),
 (13,'user13','user13@example.invalid','Stu','Student1',3,'student',1),
 (14,'user14','user14@example.invalid','Stu','Student2',3,'student',1),
 (20,'user20','user20@example.invalid','Sam','Second',5,'teacher',2),
 (21,'user21','user21@example.invalid','Sue','Student',3,'student',2),
 (22,'user22','user22@example.invalid','Sid','Student',3,'student',2),
 (30,'user30','user30@example.invalid','Eva','East',5,'teacher',3),
 (31,'user31','user31@example.invalid','Ed','Student',3,'student',3),
 (50,'user50','user50@example.invalid','Sol','Silent',5,'teacher',5),
 (51,'user51','user51@example.invalid','Sia','Student',3,'student',5),
 (60,'user60','user60@example.invalid','Demo','Teacher',5,'teacher',6),
 (61,'user61','user61@example.invalid','Demo','Student',3,'student',6),
 (70,'user70','user70@example.invalid','Facilitator','One',5,'teacher',7),
 (80,'user80','user80@example.invalid','Nora','NoSchool',5,'teacher',NULL),
 (90,'user90','user90@example.invalid','Cleo','Client',1,'client',1);

INSERT INTO subject (id, subject_code, subject_name) VALUES (1,'EN','English'),(2,'MS','Mathematics'),(3,'SP','Spanish'),(4,'UN','Unused'),(5,'OLD','Old');

-- Connections: one row per user and day.
INSERT INTO connected_date (user_id, connected_date, subscription_status) VALUES
 (11,'2026-09-01 03:59:59','active'),(11,'2026-09-01 04:00:00','active'),(11,'2026-09-02 12:00:00','active'),(11,'2026-09-03 12:00:00','active'),
 (11,'2026-09-10 12:00:00','active'),(11,'2026-10-05 12:00:00','active'),
 (12,'2026-09-02 12:00:00','active'),
 (13,'2026-09-02 12:00:00','active'),(13,'2026-09-03 12:00:00','active'),
 (14,'2026-09-15 12:00:00','active'),
 (10,'2026-09-03 12:00:00','active'),
 (20,'2026-09-02 12:00:00','active'),(20,'2026-09-03 12:00:00','active'),(20,'2026-09-04 12:00:00','active'),(20,'2026-09-05 12:00:00','active'),
 (21,'2026-09-02 12:00:00','active'),(21,'2026-10-01 03:59:59','active'),(21,'2026-10-01 04:00:00','active'),
 (30,'2026-09-02 12:00:00','active'),(30,'2026-09-30 12:00:00','active'),
 (31,'2026-09-30 12:00:00','active'),
 (60,'2026-09-02 12:00:00','active'),(70,'2026-09-02 12:00:00','active'),(80,'2026-09-02 12:00:00','active'),
 (90,'2026-09-02 12:00:00','active'),(1,'2026-09-02 12:00:00','active');

-- Tests. Rows 13 (a teacher) and 9 (excluded ORE) must be ignored. Row 11 was assigned on the local day 2026-08-31.
INSERT INTO result_generic (user_id, subject_code, quiz_assigned_to_plan_date, quiz_taken_date, quiz_title, total_percent_full_quiz) VALUES
 (13,'EN','2026-09-02 12:00:00','2026-09-04 12:00:00','Synthetic test',80.00),
 (13,'EN','2026-09-10 12:00:00',NULL,'Synthetic test',NULL),
 (14,'EN','2026-09-12 12:00:00','2026-09-12 18:00:00','Synthetic test',60.00),
 (21,'MS','2026-09-03 12:00:00','2026-09-05 12:00:00','Synthetic test',90.00),
 (22,'MS','2026-09-20 12:00:00',NULL,'Synthetic test',NULL),
 (31,'SP','2026-10-01 03:59:59','2026-10-02 12:00:00','Synthetic test',100.00),
 (31,'SP','2026-10-01 04:00:00',NULL,'Synthetic test',NULL),
 (51,'EN','2026-09-15 12:00:00',NULL,'Synthetic test',NULL),
 (61,'EN','2026-09-02 12:00:00','2026-09-03 12:00:00','Synthetic test',55.00),
 (13,'OLD','2026-02-01 12:00:00','2026-02-03 12:00:00','Synthetic test',50.00),
 (21,'EN','2026-09-01 03:59:59','2026-09-05 12:00:00','Synthetic test',70.00),
 (22,'SP','2026-09-10 12:00:00',NULL,'Synthetic test',NULL),
 (11,'EN','2026-09-05 12:00:00','2026-09-06 12:00:00','Synthetic test',99.00);

-- Subscriptions. User 11 has an active and a canceled row (active wins). User 13 is active with an empty cancellation date and user 21 with a past one.
INSERT INTO subscription_profile_history (user_id, subscription_id, status, cancellation_date) VALUES
 (10,1,'active','2099-12-31 00:00:00'),(11,1,'active','2099-12-31 00:00:00'),(11,2,'canceled','2099-12-31 00:00:00'),
 (12,1,'active','2099-12-31 00:00:00'),(13,1,'active',NULL),(14,1,'canceled','2099-12-31 00:00:00'),
 (20,1,'active','2099-12-31 00:00:00'),(21,1,'active','2000-01-01 00:00:00'),(30,1,'active','2099-12-31 00:00:00'),
 (31,1,'active','2099-12-31 00:00:00'),(50,1,'canceled','2099-12-31 00:00:00'),(51,1,'active','2099-12-31 00:00:00'),
 (1,1,'active','2099-12-31 00:00:00');

-- Teacher activity, one row per teacher and academic period of their school. Flags: connections, plans, plans with data, groups, assignments.
-- Teacher SIE11 has a duplicate row for the same period with the opposite flags: it counts ONCE and any 'SI' wins.
INSERT INTO vw_teacher_activities (sie, teacher_name, ore, school, from_period, to_period, has_connections_current_period,
  has_created_instructional_plans_current_period, plans_have_data_current_period, has_created_groups_current_year, has_created_assignments_overall) VALUES
 ('SIE11','Tom Teacher','ORE ONE','North High','2026-08-28 00:00:00','2026-10-23 00:00:00','SI','SI','NO','SI','SI'),
 ('SIE11','Tom Teacher','ORE ONE','North High','2026-08-28 00:00:00','2026-10-23 00:00:00','NO','NO','SI','NO','NO'),
 ('SIE12','Tina Teacher','ORE ONE','North High','2026-08-28 00:00:00','2026-10-23 00:00:00','NO','NO','NO','NO','SI'),
 ('SIE11','Tom Teacher','ORE ONE','North High','2026-10-24 00:00:00','2027-01-08 00:00:00','NO','NO','NO','NO','NO'),
 ('SIE20','Sam Second','ORE ONE','South High','2026-08-28 00:00:00','2026-10-23 00:00:00','SI','NO','NO','SI','NO'),
 ('SIE30','Eva East','ORE TWO','East Academy','2026-09-12 00:00:00','2026-10-15 00:00:00','SI','SI','SI','SI','SI'),
 ('SIE60','Demo Teacher','DEMOSTRACION','Demo School','2026-08-28 00:00:00','2026-10-23 00:00:00','SI','SI','SI','SI','SI'),
 ('SIE70','Facilitator One','ORE ONE','Facilitators School','2026-08-28 00:00:00','2026-10-23 00:00:00','SI','SI','SI','SI','SI');
