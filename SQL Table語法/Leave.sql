USE [HRManagementSystem]
GO

/****** Object:  Table [dbo].[Leave]    Script Date: 2025/1/22 下午 05:46:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Leave](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LeaveName] [nvarchar](20) NULL,
	[Description] [nvarchar](500) NULL,
	[LeaveLimitHours] [decimal](5, 2) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[OperateUserId] [int] NULL,
 CONSTRAINT [PK_Leave] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Leave] ADD  CONSTRAINT [DF_Leave_LeaveLimitHours]  DEFAULT ((0)) FOR [LeaveLimitHours]
GO

ALTER TABLE [dbo].[Leave] ADD  CONSTRAINT [DF_Leave_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Leave', @level2type=N'COLUMN',@level2name=N'Id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請假別名(中文)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Leave', @level2type=N'COLUMN',@level2name=N'LeaveName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'假別敘述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Leave', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最大可申請總時數(半小時為0.5)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Leave', @level2type=N'COLUMN',@level2name=N'LeaveLimitHours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作者帳號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Leave', @level2type=N'COLUMN',@level2name=N'OperateUserId'
GO


