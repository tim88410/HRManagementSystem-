USE [HRManagementSystem]
GO

/****** Object:  Table [dbo].[LeaveActionLog]    Script Date: 2025/1/22 下午 05:46:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LeaveActionLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LeaveId] [int] NOT NULL,
	[Action] [varchar](30) NOT NULL,
	[OldValue] [nvarchar](2000) NULL,
	[NewValue] [nvarchar](2000) NULL,
	[OperatorId] [int] NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_LeaveActionLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LeaveActionLog] ADD  CONSTRAINT [DF_LeaveActionLog_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Log主Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeaveActionLog', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作對象Leave的流水號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeaveActionLog', @level2type=N'COLUMN',@level2name=N'LeaveId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作的動作' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeaveActionLog', @level2type=N'COLUMN',@level2name=N'Action'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'改前的值(若為Insert可為空)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeaveActionLog', @level2type=N'COLUMN',@level2name=N'OldValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'改後的值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeaveActionLog', @level2type=N'COLUMN',@level2name=N'NewValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeaveActionLog', @level2type=N'COLUMN',@level2name=N'OperatorId'
GO


